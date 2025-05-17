using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 可互動物體的介面
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 當物體被推動時調用
        /// </summary>
        /// <param name="dir">推動方向和力度</param>
        void OnPush(Vector2 dir);
        
        /// <summary>
        /// 當物體被破壞時調用
        /// </summary>
        void OnDestroyInteractable();
    }
}
