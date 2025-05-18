using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 延遲顯示勝利UI的事件
    /// </summary>
    public class DelayedVictoryUI : Simulation.Event<DelayedVictoryUI>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        // 字體資源引用
        private static TMP_FontAsset unispaceFontAsset;

        public override void Execute()
        {
            Debug.Log("顯示勝利UI");
            ShowVictoryUI();
        }

        /// <summary>
        /// 顯示勝利UI
        /// </summary>
        void ShowVictoryUI()
        {
            // 加載字體資源
            if (unispaceFontAsset == null)
            {
                unispaceFontAsset = Resources.Load<TMP_FontAsset>("Font/UNISPACE BD SDF");

                // 如果仍然無法加載，嘗試直接使用 GUID
                if (unispaceFontAsset == null)
                {
                    // 嘗試從 Assets 目錄加載
                    unispaceFontAsset = Resources.Load<TMP_FontAsset>("Assets/Font/UNISPACE BD SDF");

                    // 如果仍然無法加載，使用默認字體
                    if (unispaceFontAsset == null)
                    {
                        Debug.LogWarning("無法加載 UNISPACE BD SDF 字體，使用默認字體");
                        unispaceFontAsset = TMP_Settings.defaultFontAsset;
                    }
                }
            }
            // 創建Canvas
            GameObject canvas = new GameObject("VictoryCanvas");
            Canvas canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();

            // 添加動畫控制器
            VictoryUIAnimator animator = canvas.AddComponent<VictoryUIAnimator>();

            // 創建Panel
            GameObject panel = new GameObject("VictoryPanel");
            panel.transform.SetParent(canvas.transform, false);
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(800, 600); // 放大至2倍

            // 添加Panel背景
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0);

            // 創建標題文本
            GameObject titleObj = new GameObject("TitleText");
            titleObj.transform.SetParent(panel.transform, false);
            RectTransform titleRect = titleObj.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.85f); // 調整位置更靠上
            titleRect.anchorMax = new Vector2(0.5f, 0.85f);
            titleRect.pivot = new Vector2(0.5f, 0.5f);
            titleRect.sizeDelta = new Vector2(700, 100); // 放大文字區域

            TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
            titleText.text = "LEVEL COMPLETE!";
            titleText.font = unispaceFontAsset; // 使用加載的字體資源
            titleText.fontSize = 72; // 放大字體
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = new Color(1, 1, 1, 0); // 初始透明

            // 創建救援計數文本
            GameObject rescuedObj = new GameObject("RescuedText");
            rescuedObj.transform.SetParent(panel.transform, false);
            RectTransform rescuedRect = rescuedObj.AddComponent<RectTransform>();
            rescuedRect.anchorMin = new Vector2(0.5f, 0.55f); // 調整位置
            rescuedRect.anchorMax = new Vector2(0.5f, 0.55f);
            rescuedRect.pivot = new Vector2(0.5f, 0.5f);
            rescuedRect.sizeDelta = new Vector2(700, 80); // 放大文字區域

            // 計算實際解救的NPC數量
            int actualRescuedCount = 0;
            var allTargets = Object.FindObjectsOfType<Mechanics.RescueTarget>();
            Debug.Log($"找到 {allTargets.Length} 個 RescueTarget 物件");

            foreach (var target in allTargets)
            {
                Debug.Log($"RescueTarget: {target.name}, isRescued: {target.isRescued}, isFollowing: {target.isFollowing}");
                if (target.isRescued)
                {
                    actualRescuedCount++;
                }
            }

            Debug.Log($"實際解救的NPC數量: {actualRescuedCount}");

            // 更新model中的計數，確保其他地方使用時也是正確的
            model.rescuedCount = actualRescuedCount;
            Debug.Log($"更新後的model.rescuedCount: {model.rescuedCount}");

            TextMeshProUGUI rescuedText = rescuedObj.AddComponent<TextMeshProUGUI>();
            rescuedText.text = $"YOU RESCUED {actualRescuedCount} INNOCENT BUNNIES";
            rescuedText.font = unispaceFontAsset; // 使用加載的字體資源
            rescuedText.fontSize = 48; // 放大字體
            rescuedText.alignment = TextAlignmentOptions.Center;
            rescuedText.color = new Color(1, 1, 1, 0); // 初始透明

            // 創建繼續按鈕
            GameObject buttonObj = new GameObject("ContinueButton");
            buttonObj.transform.SetParent(panel.transform, false);
            RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.25f); // 調整位置
            buttonRect.anchorMax = new Vector2(0.5f, 0.25f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
            buttonRect.sizeDelta = new Vector2(400, 100); // 放大按鈕

            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 1f, 0f); // 初始透明

            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = buttonImage;

            // 創建按鈕文本
            GameObject buttonTextObj = new GameObject("ButtonText");
            buttonTextObj.transform.SetParent(buttonObj.transform, false);
            RectTransform buttonTextRect = buttonTextObj.AddComponent<RectTransform>();
            buttonTextRect.anchorMin = Vector2.zero;
            buttonTextRect.anchorMax = Vector2.one;
            buttonTextRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI buttonText = buttonTextObj.AddComponent<TextMeshProUGUI>();
            buttonText.text = "CONTINUE";
            buttonText.font = unispaceFontAsset; // 使用加載的字體資源
            buttonText.fontSize = 48; // 放大字體
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.color = new Color(1, 1, 1, 0); // 初始透明

            // 設置按鈕事件
            button.onClick.AddListener(() => {
                // 返回主選單或重新開始遊戲
                try {
                    SceneManager.LoadScene("StartScene");
                } catch (System.Exception e) {
                    Debug.LogWarning("無法載入場景: " + e.Message);
                    // 如果無法載入場景，則銷毀UI
                    GameObject.Destroy(canvas);
                }
            });

            // 確保有EventSystem
            if (EventSystem.current == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
                Debug.Log("創建EventSystem");
            }

            // 設置動畫控制器參數
            animator.panel = panel;
            animator.panelImage = panelImage;
            animator.titleText = titleText;
            animator.rescuedText = rescuedText;
            animator.buttonImage = buttonImage;
            animator.buttonText = buttonText;
            animator.button = button;

            // 開始動畫
            animator.StartAnimations();
        }
    }
}
