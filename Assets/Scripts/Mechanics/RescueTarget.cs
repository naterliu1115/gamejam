using System;
using System.Collections;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 可救援的 NPC 目標
    /// </summary>
    public class RescueTarget : MonoBehaviour
    {
        public event Action<RescueTarget> OnFollowStarted;
        public event Action<RescueTarget> OnStay;

        public float followSpeed = 3f;
        public float followDistance = 1.5f;
        public int healthPoints = 3;
        public bool isFollowing = false;
        public bool isRescued = false;

        private PlayerController player;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private Collider2D col;

        void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        void Update()
        {
            if (isFollowing && player != null)
            {
                // 跟隨玩家
                var targetPosition = player.transform.position - new Vector3(followDistance * (player.GetComponent<SpriteRenderer>().flipX ? -1 : 1), 0, 0);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);

                // 更新朝向
                spriteRenderer.flipX = transform.position.x > player.transform.position.x;

                // 更新動畫
                animator.SetFloat("velocityX", Mathf.Abs(transform.position.x - targetPosition.x));
            }
        }

        /// <summary>
        /// 當 NPC 被救援時調用
        /// </summary>
        public void OnRescue(PlayerController rescuer)
        {
            if (isFollowing || isRescued) return;

            player = rescuer;
            isFollowing = true;
            animator.SetTrigger("rescued");

            // 觸發事件
            OnFollowStarted?.Invoke(this);
        }

        /// <summary>
        /// 讓 NPC 停留在當前位置
        /// </summary>
        public void Stay()
        {
            if (!isFollowing) return;

            isFollowing = false;
            isRescued = true;
            animator.SetTrigger("stay");

            // 觸發事件
            OnStay?.Invoke(this);

            // 更新救援計數
            var model = Simulation.GetModel<PlatformerModel>();
            model.rescuedCount++;

            // 檢查是否所有目標都已救援
            CheckAllRescued();
        }

        /// <summary>
        /// 對 NPC 造成傷害
        /// </summary>
        public void TakeDamage(int amount)
        {
            healthPoints -= amount;
            animator.SetTrigger("hurt");

            if (healthPoints <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// NPC 死亡
        /// </summary>
        void Die()
        {
            animator.SetTrigger("die");
            isFollowing = false;

            // 禁用碰撞和物理
            col.enabled = false;
            rb.simulated = false;

            // 檢查是否所有目標都已死亡
            CheckAllDead();

            // 延遲銷毀
            Destroy(gameObject, 2f);
        }

        /// <summary>
        /// 檢查是否所有目標都已救援
        /// </summary>
        void CheckAllRescued()
        {
            var model = Simulation.GetModel<PlatformerModel>();
            if (model.rescuedCount == model.maxRescueTargets)
            {
                // 觸發關卡成功事件
                Simulation.Schedule<PlayerEnteredVictoryZone>();
            }
        }

        /// <summary>
        /// 檢查是否所有目標都已死亡
        /// </summary>
        void CheckAllDead()
        {
            var allTargets = FindObjectsOfType<RescueTarget>();
            var allDead = true;

            foreach (var target in allTargets)
            {
                if (target.healthPoints > 0 && !target.isRescued)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                // 觸發關卡失敗事件
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }
}
