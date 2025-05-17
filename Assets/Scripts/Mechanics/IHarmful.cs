using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 有害物體的介面
    /// </summary>
    public interface IHarmful
    {
        /// <summary>
        /// 對玩家應用效果
        /// </summary>
        /// <param name="player">玩家控制器</param>
        void ApplyEffect(PlayerController player);
    }
}
