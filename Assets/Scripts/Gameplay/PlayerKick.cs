using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 當玩家執行踢擊動作時觸發此事件
    /// </summary>
    public class PlayerKick : Simulation.Event<PlayerKick>
    {
        public PlayerController player;
        public GameObject target;
        
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        
        public override void Execute()
        {
            // 播放踢擊音效
            // 如果有踢擊音效，可以在這裡播放
            // if (player.audioSource && player.kickAudio)
            //     player.audioSource.PlayOneShot(player.kickAudio);
                
            // 踢擊效果，如粒子效果等
            // 可以在這裡實例化粒子效果
            
            // 如果目標是敵人，可以在這裡處理敵人受傷邏輯
            var enemy = target?.GetComponent<EnemyController>();
            if (enemy != null)
            {
                var health = enemy.GetComponent<Health>();
                if (health != null)
                {
                    health.Decrement();
                    if (!health.IsAlive)
                    {
                        // 增加擊敗敵人的計數
                        model.defeatedMinorEnemies++;
                    }
                }
            }
            
            // 如果目標是可互動物體，可以在這裡處理物體破壞邏輯
            var interactable = target?.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnDestroyInteractable();
                // 增加破壞物體的計數
                model.destroyedObjects++;
            }
        }
    }
}
