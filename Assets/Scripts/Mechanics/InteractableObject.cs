using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 可互動物體的基類
    /// </summary>
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        public bool isPushable = true;
        public bool isDestructible = true;
        public float pushResistance = 1f;
        public GameObject destroyEffect;
        public AudioClip pushSound;
        public AudioClip destroySound;
        
        protected Rigidbody2D rb;
        protected AudioSource audioSource;
        
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            
            // 如果沒有 AudioSource，則添加一個
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        /// <summary>
        /// 當物體被推動時調用
        /// </summary>
        public virtual void OnPush(Vector2 dir)
        {
            if (!isPushable) return;
            
            if (audioSource && pushSound)
                audioSource.PlayOneShot(pushSound);
                
            if (rb != null)
            {
                // 根據推動力和阻力計算實際力度
                var model = Simulation.GetModel<PlatformerModel>();
                float actualForce = model.pushForce / pushResistance;
                rb.AddForce(dir * actualForce, ForceMode2D.Impulse);
            }
        }
        
        /// <summary>
        /// 當物體被破壞時調用
        /// </summary>
        public virtual void OnDestroyInteractable()
        {
            if (!isDestructible) return;
            
            if (audioSource && destroySound)
                audioSource.PlayOneShot(destroySound);
                
            if (destroyEffect != null)
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
                
            // 增加分數
            var model = Simulation.GetModel<PlatformerModel>();
            model.destroyedObjects++;
            model.score += model.objectDestroyScore;
            
            // 延遲銷毀以播放音效
            Destroy(gameObject, destroySound ? destroySound.length : 0.1f);
        }
    }
}
