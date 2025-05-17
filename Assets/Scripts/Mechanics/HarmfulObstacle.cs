using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 有害障礙物的基類
    /// </summary>
    public class HarmfulObstacle : MonoBehaviour, IHarmful
    {
        public enum EffectType
        {
            Damage,
            Stun,
            Both
        }

        public EffectType effectType = EffectType.Damage;
        public int damageAmount = 1;
        public float stunDuration = 2f;
        public AudioClip effectSound;
        public GameObject effectPrefab;

        protected AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            // 如果沒有 AudioSource，則添加一個
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // 檢查是否是玩家
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                ApplyEffect(player);
            }

            // 檢查是否是 NPC
            var npc = other.GetComponent<RescueTarget>();
            if (npc != null)
            {
                // 對 NPC 造成傷害
                if (effectType == EffectType.Damage || effectType == EffectType.Both)
                {
                    npc.TakeDamage(damageAmount);
                }
            }
        }

        /// <summary>
        /// 對玩家應用效果
        /// </summary>
        public virtual void ApplyEffect(PlayerController player)
        {
            // 播放音效
            if (audioSource && effectSound)
                audioSource.PlayOneShot(effectSound);

            // 生成效果
            if (effectPrefab != null)
                Instantiate(effectPrefab, player.transform.position, Quaternion.identity);

            // 應用效果
            switch (effectType)
            {
                case EffectType.Damage:
                    // 對玩家造成傷害
                    var health = player.GetComponent<Health>();
                    if (health != null)
                    {
                        // 多次調用 Decrement 方法來減少多個生命值
                        for (int i = 0; i < damageAmount; i++)
                            health.Decrement();
                    }
                    break;

                case EffectType.Stun:
                    // 使玩家暈眩
                    player.ApplyStun(stunDuration);
                    break;

                case EffectType.Both:
                    // 同時造成傷害和暈眩
                    var playerHealth = player.GetComponent<Health>();
                    if (playerHealth != null)
                    {
                        // 多次調用 Decrement 方法來減少多個生命值
                        for (int i = 0; i < damageAmount; i++)
                            playerHealth.Decrement();
                    }
                    player.ApplyStun(stunDuration);
                    break;
            }
        }
    }
}
