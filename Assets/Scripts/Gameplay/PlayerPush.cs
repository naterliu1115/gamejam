using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 當玩家開始推動物體時觸發此事件
    /// </summary>
    public class PlayerPush : Simulation.Event<PlayerPush>
    {
        public PlayerController player;
        public GameObject pushable;
        
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        
        public override void Execute()
        {
            // 播放推動音效
            // 如果有推動音效，可以在這裡播放
            // if (player.audioSource && player.pushAudio)
            //     player.audioSource.PlayOneShot(player.pushAudio);
                
            // 推動效果，如粒子效果等
            // 可以在這裡實例化粒子效果
            
            // 如果目標是可互動物體，可以在這裡處理物體推動邏輯
            var interactable = pushable?.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // 獲取玩家朝向
                float direction = player.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
                interactable.OnPush(new Vector2(direction, 0) * model.pushForce);
            }
        }
    }
}
