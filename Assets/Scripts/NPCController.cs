using UnityEngine;

namespace Platformer.Mechanics
{
    public class NPCController : KinematicObject
    {
        /// <summary> 跟隨的目標 如果是Null則代表沒目標 </summary>
        public PlayerController Player;

        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            bool IsMove = false;
            if (Player != null)
            {
                float D = Vector3.Distance(transform.position, Player.transform.position);
                //沒有太近
                if (D > 0.5f)
                {
                    float XD = Mathf.Abs(transform.position.x - Player.transform.position.x);
                    if (XD > 0.5f)
                    {
                        //太遠了 並且玩家洛地
                        if (D > 6f && Player.IsGrounded)
                        {
                            //順移
                            transform.position = Player.transform.position;
                        }
                        else
                        {
                            //print(D);
                            //左邊還是右邊
                            if (transform.position.x > Player.transform.position.x)
                            {
                                targetVelocity.x = -2.8f;
                            }
                            else
                            {
                                targetVelocity.x = 2.8f;
                            }
                            //print(targetVelocity.x);
                            IsMove = true;
                        }
                    }
                }
            }
            if (!IsMove)
            {
                targetVelocity = Vector2.zero;
            }
            //base.Update();
        }
    }
}