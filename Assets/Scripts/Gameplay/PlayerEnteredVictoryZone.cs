using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Platformer.Gameplay
{
    /// <summary>
    /// This event is triggered when the player character enters a trigger with a VictoryZone component.
    /// </summary>
    /// <typeparam name="PlayerEnteredVictoryZone"></typeparam>
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            // 設置勝利動畫
            model.player.animator.SetTrigger("victory");
            model.player.controlEnabled = false;

            // 相機簡單效果 - 稍微放大
            if (model.virtualCamera != null)
            {
                // 使用簡單的直接設置 - 使用更安全的方式訪問相機
                try
                {
                    // 嘗試獲取相機組件並調整視野
                    var camera = Camera.main;
                    if (camera != null && camera.orthographic)
                    {
                        camera.orthographicSize *= 0.9f;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("無法調整相機大小: " + e.Message);
                }
            }

            // 顯示勝利UI
            Debug.Log("Victory zone triggered! Showing UI...");

            // 延遲顯示UI
            Debug.Log("計劃1.5秒後顯示勝利UI");
            Simulation.Schedule<DelayedVictoryUI>(1.5f);

            // 添加EventSystem（如果不存在）
            if (GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("Created EventSystem");
            }

            // 如果有粒子特效，播放它
            if (victoryZone != null && victoryZone.victoryParticles != null)
            {
                victoryZone.victoryParticles.Play();
                Debug.Log("Playing victory particles");
            }
        }


    }
}