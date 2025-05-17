using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 可拾取物品的基類
    /// </summary>
    public class PickupItem : MonoBehaviour
    {
        public enum PickupType
        {
            Health,
            Score,
            Stun,
            Damage
        }

        public PickupType pickupType = PickupType.Health;
        public int healthAmount = 1;
        public int scoreAmount = 100;
        public float stunDuration = 2f;
        public int damageAmount = 1;

        public AudioClip pickupSound;
        public GameObject pickupEffect;

        void OnTriggerEnter2D(Collider2D other)
        {
            // 檢查是否是玩家
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // 應用效果
                ApplyEffect(player);

                // 播放音效
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                // 生成效果
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                // 銷毀物品
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 對玩家應用效果
        /// </summary>
        void ApplyEffect(PlayerController player)
        {
            var model = Simulation.GetModel<PlatformerModel>();

            switch (pickupType)
            {
                case PickupType.Health:
                    // 恢復生命值
                    var health = player.GetComponent<Health>();
                    if (health != null)
                    {
                        // 多次調用 Increment 方法來增加多個生命值
                        for (int i = 0; i < healthAmount; i++)
                            health.Increment();
                    }
                    break;

                case PickupType.Score:
                    // 增加分數
                    model.score += scoreAmount;
                    break;

                case PickupType.Stun:
                    // 使玩家暈眩
                    player.ApplyStun(stunDuration);
                    break;

                case PickupType.Damage:
                    // 對玩家造成傷害
                    var playerHealth = player.GetComponent<Health>();
                    if (playerHealth != null)
                    {
                        // 多次調用 Decrement 方法來減少多個生命值
                        for (int i = 0; i < damageAmount; i++)
                            playerHealth.Decrement();
                    }
                    break;
            }
        }
    }
}
