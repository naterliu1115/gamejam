using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class VictoryZone : MonoBehaviour
    {
        /// <summary>
        /// 勝利粒子特效
        /// </summary>
        public ParticleSystem victoryParticles;

        /// <summary>
        /// 勝利音效
        /// </summary>
        public AudioClip victorySoundEffect;

        /// <summary>
        /// 粒子特效持續時間
        /// </summary>
        public float particlesDuration = 3f;

        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                // 播放粒子特效
                if (victoryParticles != null)
                {
                    victoryParticles.Play();
                    // 確保粒子系統在一段時間後停止
                    Invoke("StopParticles", particlesDuration);
                }

                // 播放勝利音效
                if (victorySoundEffect != null)
                {
                    AudioSource.PlayClipAtPoint(victorySoundEffect, transform.position);
                }

                // 觸發勝利事件
                var ev = Schedule<PlayerEnteredVictoryZone>();
                ev.victoryZone = this;
            }
        }

        /// <summary>
        /// 停止粒子特效
        /// </summary>
        void StopParticles()
        {
            if (victoryParticles != null)
                victoryParticles.Stop();
        }
    }
}