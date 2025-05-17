using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 當玩家執行壁跳時觸發此事件
    /// </summary>
    public class PlayerWallJump : Simulation.Event<PlayerWallJump>
    {
        public PlayerController player;
        
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        
        public override void Execute()
        {
            // 播放音效
            if (player.audioSource && player.jumpAudio)
                player.audioSource.PlayOneShot(player.jumpAudio);
                
            // 可以在這裡添加其他壁跳效果，如粒子效果等
        }
    }
}
