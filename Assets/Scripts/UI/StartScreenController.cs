using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#endif

namespace Platformer.UI
{
    /// <summary>
    /// 控制開始畫面的行為
    /// </summary>
    public class StartScreenController : MonoBehaviour
    {
        /// <summary>
        /// 場景控制器引用
        /// </summary>
        [SerializeField] private SceneController sceneController;

        /// <summary>
        /// 開始遊戲按鈕
        /// </summary>
        [SerializeField] private Button startButton;

        /// <summary>
        /// 退出遊戲按鈕
        /// </summary>
        [SerializeField] private Button quitButton;

        /// <summary>
        /// 背景圖片
        /// </summary>
        [SerializeField] private Image backgroundImage;

        /// <summary>
        /// 標題文字
        /// </summary>
        [SerializeField] private TMPro.TextMeshProUGUI titleText;

        void Start()
        {
            // 確保場景控制器已設置
            if (sceneController == null)
            {
                sceneController = FindObjectOfType<SceneController>();
                if (sceneController == null)
                {
                    Debug.LogError("無法找到 SceneController，請確保場景中有此組件");
                }
            }

            // 設置按鈕事件
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartButtonClicked);
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitButtonClicked);
            }

            // 確保場景中有 EventSystem
            if (FindObjectOfType<EventSystem>() == null)
            {
                Debug.Log("創建 EventSystem");
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();

                #if ENABLE_INPUT_SYSTEM
                // 使用新的 Input System
                eventSystem.AddComponent<InputSystemUIInputModule>();
                #else
                // 使用舊的 Input Manager
                eventSystem.AddComponent<StandaloneInputModule>();
                #endif
            }
        }

        /// <summary>
        /// 開始按鈕點擊事件
        /// </summary>
        public void OnStartButtonClicked()
        {
            if (sceneController != null)
            {
                sceneController.LoadGameScene();
            }
        }

        /// <summary>
        /// 退出按鈕點擊事件
        /// </summary>
        public void OnQuitButtonClicked()
        {
            if (sceneController != null)
            {
                sceneController.QuitGame();
            }
        }
    }
}
