using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    /// <summary>
    /// 負責管理場景切換的控制器
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// 遊戲場景的名稱
        /// </summary>
        [SerializeField] private string gameSceneName = "SampleScene";

        /// <summary>
        /// 開始場景的名稱
        /// </summary>
        [SerializeField] private string startSceneName = "StartScene";

        /// <summary>
        /// 切換到遊戲場景
        /// </summary>
        public void LoadGameScene()
        {
            SceneManager.LoadScene(gameSceneName);
        }

        /// <summary>
        /// 切換到開始場景
        /// </summary>
        public void LoadStartScene()
        {
            SceneManager.LoadScene(startSceneName);
        }

        /// <summary>
        /// 退出遊戲
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
