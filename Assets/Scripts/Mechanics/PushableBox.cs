using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 可推動的箱子
    /// </summary>
    public class PushableBox : InteractableObject
    {
        public float maxPushSpeed = 3f;
        public float friction = 0.95f;
        
        protected override void Awake()
        {
            base.Awake();
            
            // 確保有 Rigidbody2D
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.mass = 2f;
                rb.linearDamping = 2f;
                rb.angularDamping = 0.5f;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            
            // 設置為可推動但不可破壞
            isPushable = true;
            isDestructible = false;
        }
        
        void FixedUpdate()
        {
            // 限制最大速度
            if (rb.linearVelocity.magnitude > maxPushSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxPushSpeed;
            }
            
            // 應用摩擦力
            rb.linearVelocity *= friction;
            
            // 如果速度很小，停止移動
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        
        public override void OnPush(Vector2 dir)
        {
            base.OnPush(dir);
            
            // 可以在這裡添加推動時的特殊效果
            // 例如播放推動音效、生成粒子效果等
        }
    }
}
