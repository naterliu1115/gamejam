using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        // 壁跳相關
        public bool IsWallSliding { get; private set; }
        private bool isTouchingWall = false;
        private float wallCheckDistance = 0.2f;

        // 推動相關
        public bool isPushing = false;
        private GameObject pushableObject = null;

        // 踢擊相關
        public bool isKicking = false;
        private float kickCooldownTimer = 0f;

        // 暈眩相關
        public bool isStunned = false;
        private float stunTimer = 0f;

        // 輸入操作引用
        private InputAction m_MoveAction;
        private InputAction m_JumpAction;
        private InputAction m_DropDownAction;
        private InputAction m_PushAction;
        private InputAction m_KickAction;
        private InputAction m_RescueAction;

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            m_MoveAction = InputSystem.actions.FindAction("Player/Move");
            m_JumpAction = InputSystem.actions.FindAction("Player/Jump");
            m_DropDownAction = InputSystem.actions.FindAction("Player/DropDown");
            m_PushAction = InputSystem.actions.FindAction("Player/Push");
            m_KickAction = InputSystem.actions.FindAction("Player/Kick");
            m_RescueAction = InputSystem.actions.FindAction("Player/Rescue");

            m_MoveAction.Enable();
            m_JumpAction.Enable();
            m_DropDownAction.Enable();
            m_PushAction.Enable();
            m_KickAction.Enable();
            m_RescueAction.Enable();
        }

        protected override void Update()
        {
            // 處理暈眩狀態
            if (isStunned)
            {
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0)
                {
                    isStunned = false;
                    controlEnabled = true;
                }
            }

            // 更新踢擊冷卻
            if (kickCooldownTimer > 0)
                kickCooldownTimer -= Time.deltaTime;

            if (controlEnabled && !isStunned)
            {
                // 基本移動和跳躍
                move.x = m_MoveAction.ReadValue<Vector2>().x;
                if (jumpState == JumpState.Grounded && m_JumpAction.WasPressedThisFrame())
                    jumpState = JumpState.PrepareToJump;
                else if (m_JumpAction.WasReleasedThisFrame())
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }

                // 檢測壁跳
                CheckWallSliding();

                // 處理壁跳
                if (IsWallSliding && m_JumpAction.WasPressedThisFrame())
                {
                    PerformWallJump();
                }

                // 處理下穿平台
                if (IsGrounded && m_DropDownAction.WasPressedThisFrame())
                {
                    HandleDropDown();
                }

                // 處理推動
                if (m_PushAction.IsPressed())
                {
                    HandlePush();
                }
                else if (isPushing)
                {
                    StopPushing();
                }

                // 處理踢擊
                if (m_KickAction.WasPressedThisFrame() && kickCooldownTimer <= 0)
                {
                    HandleKick();
                }

                // 處理救援
                if (m_RescueAction.WasPressedThisFrame())
                {
                    HandleRescue();
                }
            }
            else
            {
                move.x = 0;
            }

            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        #region 壁跳功能
        // 檢測壁跳
        void CheckWallSliding()
        {
            // 只有在空中且有水平輸入時才檢測壁跳
            if (!IsGrounded && Mathf.Abs(move.x) > 0.1f)
            {
                // 檢測前方是否有牆壁
                isTouchingWall = Physics2D.Raycast(
                    transform.position,
                    new Vector2(move.x, 0),
                    wallCheckDistance,
                    LayerMask.GetMask("Ground")
                );

                // 如果接觸牆壁且不在地面上，則進入壁滑狀態
                IsWallSliding = isTouchingWall;

                if (IsWallSliding)
                {
                    // 減緩下落速度
                    if (velocity.y < 0)
                        velocity.y = -model.wallSlideSpeed;

                    // 更新動畫
                    animator.SetBool("wallSliding", true);
                }
                else
                {
                    animator.SetBool("wallSliding", false);
                }
            }
            else
            {
                IsWallSliding = false;
                animator.SetBool("wallSliding", false);
            }
        }

        // 執行壁跳
        void PerformWallJump()
        {
            // 從牆壁反方向跳躍
            velocity.y = jumpTakeOffSpeed * model.jumpModifier;
            velocity.x = -move.x * model.wallJumpForce;

            // 重置壁滑狀態
            IsWallSliding = false;
            animator.SetBool("wallSliding", false);

            // 觸發跳躍動畫
            jumpState = JumpState.Jumping;

            // 發送壁跳事件
            var ev = Schedule<PlayerWallJump>();
            ev.player = this;
        }
        #endregion

        #region 下穿平台功能
        // 處理下穿平台
        void HandleDropDown()
        {
            // 檢測下方是否有單向平台
            var platform = CheckPlatformBelow();
            if (platform != null)
            {
                StartCoroutine(DisablePlatformCollision(platform));
            }
        }

        // 檢測下方平台
        Collider2D CheckPlatformBelow()
        {
            // 使用 OverlapCircle 檢測下方的平台
            var colliders = Physics2D.OverlapCircleAll(
                transform.position - new Vector3(0, 0.1f, 0),
                0.2f,
                LayerMask.GetMask("OneWayPlatform")
            );

            // 返回第一個找到的平台
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<PlatformEffector2D>() != null)
                    return collider;
            }

            return null;
        }

        // 暫時禁用平台碰撞
        IEnumerator DisablePlatformCollision(Collider2D platform)
        {
            // 獲取平台的 PlatformEffector2D
            var effector = platform.GetComponent<PlatformEffector2D>();
            if (effector != null)
            {
                // 暫時改變碰撞方向
                float originalRotation = effector.rotationalOffset;
                effector.rotationalOffset = 180f;

                // 暫時禁用與玩家的碰撞
                Physics2D.IgnoreCollision(collider2d, platform, true);

                // 等待短暫時間
                yield return new WaitForSeconds(0.3f);

                // 恢復碰撞
                Physics2D.IgnoreCollision(collider2d, platform, false);

                // 恢復原始設置
                effector.rotationalOffset = originalRotation;
            }
        }
        #endregion

        #region 推動功能
        // 處理推動
        void HandlePush()
        {
            if (!isPushing)
            {
                // 檢測可推動物體
                pushableObject = CheckPushableObject();
                if (pushableObject != null)
                {
                    isPushing = true;
                    animator.SetBool("pushing", true);

                    // 發送推動事件
                    var ev = Schedule<PlayerPush>();
                    ev.player = this;
                    ev.pushable = pushableObject;
                }
            }
            else if (pushableObject != null)
            {
                // 持續推動物體
                var interactable = pushableObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnPush(new Vector2(move.x, 0) * model.pushForce);
                }
            }
        }

        // 停止推動
        void StopPushing()
        {
            isPushing = false;
            pushableObject = null;
            animator.SetBool("pushing", false);
        }

        // 檢測可推動物體
        GameObject CheckPushableObject()
        {
            // 檢測前方是否有可推動物體
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                new Vector2(move.x, 0),
                0.7f,
                LayerMask.GetMask("Pushable")
            );

            return hit.collider?.gameObject;
        }
        #endregion

        #region 踢擊功能
        // 處理踢擊
        void HandleKick()
        {
            isKicking = true;
            kickCooldownTimer = model.kickCooldown;

            // 觸發踢擊動畫
            animator.SetTrigger("kick");

            // 檢測可踢擊物體或敵人
            var kickTarget = CheckKickTarget();
            if (kickTarget != null)
            {
                // 發送踢擊事件
                var ev = Schedule<PlayerKick>();
                ev.player = this;
                ev.target = kickTarget;
            }

            // 重置踢擊狀態
            StartCoroutine(ResetKickState());
        }

        // 重置踢擊狀態
        IEnumerator ResetKickState()
        {
            yield return new WaitForSeconds(0.3f); // 動畫持續時間
            isKicking = false;
        }

        // 檢測踢擊目標
        GameObject CheckKickTarget()
        {
            // 檢測前方是否有可踢擊物體
            float direction = spriteRenderer.flipX ? -1 : 1;
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                new Vector2(direction, 0),
                1.0f,
                LayerMask.GetMask("Kickable", "Enemy")
            );

            return hit.collider?.gameObject;
        }
        #endregion

        #region 救援功能
        // 處理救援
        void HandleRescue()
        {
            // 檢測可救援目標
            var rescueTarget = CheckRescueTarget();
            if (rescueTarget != null)
            {
                // 觸發救援動畫
                animator.SetTrigger("rescue");

                // 調用救援目標的方法
                rescueTarget.OnRescue(this);
            }
        }

        // 檢測救援目標
        RescueTarget CheckRescueTarget()
        {
            // 檢測周圍是否有可救援目標
            var colliders = Physics2D.OverlapCircleAll(
                transform.position,
                1.5f,
                LayerMask.GetMask("NPC")
            );

            // 返回第一個找到的救援目標
            foreach (var collider in colliders)
            {
                var rescueTarget = collider.GetComponent<RescueTarget>();
                if (rescueTarget != null && !rescueTarget.isFollowing && !rescueTarget.isRescued)
                    return rescueTarget;
            }

            return null;
        }
        #endregion

        #region 暈眩功能
        // 應用暈眩效果
        public void ApplyStun(float duration)
        {
            isStunned = true;
            controlEnabled = false;
            stunTimer = duration;
            animator.SetTrigger("stun");
        }
        #endregion
    }
}