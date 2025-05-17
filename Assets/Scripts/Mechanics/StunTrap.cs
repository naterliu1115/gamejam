using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 暈眩陷阱
    /// </summary>
    public class StunTrap : HarmfulObstacle
    {
        public float activationDelay = 0.5f;
        public float resetTime = 3f;
        
        private bool isActive = true;
        private SpriteRenderer spriteRenderer;
        
        void Start()
        {
            // 設置為暈眩類型
            effectType = EffectType.Stun;
            stunDuration = 2f;
            
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public override void ApplyEffect(PlayerController player)
        {
            if (!isActive) return;
            
            base.ApplyEffect(player);
            
            // 暫時禁用陷阱
            isActive = false;
            
            // 視覺反饋
            if (spriteRenderer != null)
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                
            // 延遲重置
            Invoke("ResetTrap", resetTime);
        }
        
        void ResetTrap()
        {
            isActive = true;
            
            // 視覺反饋
            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;
        }
    }
}
