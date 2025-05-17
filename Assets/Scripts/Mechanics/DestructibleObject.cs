using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 可破壞的物體
    /// </summary>
    public class DestructibleObject : InteractableObject
    {
        public GameObject[] dropItems;
        public float dropChance = 0.5f;
        
        protected override void Awake()
        {
            base.Awake();
            
            // 設置為不可推動但可破壞
            isPushable = false;
            isDestructible = true;
        }
        
        public override void OnDestroyInteractable()
        {
            base.OnDestroyInteractable();
            
            // 隨機掉落物品
            if (dropItems != null && dropItems.Length > 0 && Random.value <= dropChance)
            {
                int index = Random.Range(0, dropItems.Length);
                if (dropItems[index] != null)
                {
                    Instantiate(dropItems[index], transform.position, Quaternion.identity);
                }
            }
        }
    }
}
