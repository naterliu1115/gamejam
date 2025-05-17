using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 火焰陷阱
    /// </summary>
    public class FireHazard : HarmfulObstacle
    {
        public ParticleSystem fireParticles;
        
        void Start()
        {
            // 設置為傷害類型
            effectType = EffectType.Damage;
            damageAmount = 1;
            
            // 如果沒有指定粒子效果，則創建一個
            if (fireParticles == null)
            {
                var particleObj = new GameObject("FireParticles");
                particleObj.transform.SetParent(transform);
                particleObj.transform.localPosition = Vector3.zero;
                
                fireParticles = particleObj.AddComponent<ParticleSystem>();
                
                // 設置粒子系統參數
                var main = fireParticles.main;
                main.startColor = new Color(1f, 0.5f, 0f, 1f);
                main.startSize = 0.5f;
                main.startLifetime = 1f;
                main.startSpeed = 1f;
                main.maxParticles = 100;
                
                var emission = fireParticles.emission;
                emission.rateOverTime = 20f;
                
                var shape = fireParticles.shape;
                shape.shapeType = ParticleSystemShapeType.Circle;
                shape.radius = 0.5f;
            }
        }
        
        public override void ApplyEffect(PlayerController player)
        {
            base.ApplyEffect(player);
            
            // 可以在這裡添加火焰特有的效果
            // 例如播放燃燒音效、生成燃燒粒子等
        }
    }
}
