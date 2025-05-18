using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 勝利UI動畫控制器
    /// </summary>
    public class VictoryUIAnimator : MonoBehaviour
    {
        // UI元素引用
        [HideInInspector] public GameObject panel;
        [HideInInspector] public Image panelImage;
        [HideInInspector] public TextMeshProUGUI titleText;
        [HideInInspector] public TextMeshProUGUI rescuedText;
        [HideInInspector] public Image buttonImage;
        [HideInInspector] public TextMeshProUGUI buttonText;
        [HideInInspector] public Button button;

        // 動畫參數
        private float fadeInDuration = 0.7f; // 增加淡入時間
        private float textAnimDelay = 0.4f; // 增加延遲
        private float buttonAnimDelay = 1.0f; // 增加按鈕延遲
        private float pulseDuration = 2.0f; // 增加脈動週期

        /// <summary>
        /// 開始所有動畫
        /// </summary>
        public void StartAnimations()
        {
            // 禁用按鈕直到動畫完成
            if (button != null)
                button.interactable = false;

            // 開始面板淡入動畫
            StartCoroutine(FadeInPanel());

            // 開始標題動畫
            StartCoroutine(AnimateTitle());

            // 開始救援文本動畫
            StartCoroutine(AnimateRescuedText());

            // 開始按鈕動畫
            StartCoroutine(AnimateButton());
        }

        /// <summary>
        /// 面板淡入動畫
        /// </summary>
        private IEnumerator FadeInPanel()
        {
            float elapsedTime = 0;
            Color startColor = panelImage.color;
            Color targetColor = new Color(0, 0, 0, 0.8f);

            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeInDuration);
                panelImage.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            panelImage.color = targetColor;
        }

        /// <summary>
        /// 標題動畫
        /// </summary>
        private IEnumerator AnimateTitle()
        {
            // 等待一小段時間
            yield return new WaitForSeconds(textAnimDelay);

            // 淡入動畫
            float elapsedTime = 0;
            Color startColor = titleText.color;
            Color targetColor = Color.white;

            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeInDuration);
                titleText.color = Color.Lerp(startColor, targetColor, t);

                // 同時縮放 - 增加縮放範圍
                float scale = Mathf.Lerp(0.5f, 1.4f, t);
                titleText.transform.localScale = new Vector3(scale, scale, 1);
                yield return null;
            }

            titleText.color = targetColor;

            // 彈回動畫
            elapsedTime = 0;
            float bounceDuration = 0.2f;

            while (elapsedTime < bounceDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / bounceDuration);
                float scale = Mathf.Lerp(1.4f, 1f, t);
                titleText.transform.localScale = new Vector3(scale, scale, 1);
                yield return null;
            }

            titleText.transform.localScale = Vector3.one;

            // 開始脈動動畫
            StartCoroutine(PulseText(titleText, 1.1f, pulseDuration)); // 增加脈動幅度
        }

        /// <summary>
        /// 救援文本動畫
        /// </summary>
        private IEnumerator AnimateRescuedText()
        {
            // 等待比標題更長的時間
            yield return new WaitForSeconds(textAnimDelay * 2);

            // 淡入動畫
            float elapsedTime = 0;
            Color startColor = rescuedText.color;
            Color targetColor = Color.white;

            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeInDuration);
                rescuedText.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            rescuedText.color = targetColor;
        }

        /// <summary>
        /// 按鈕動畫
        /// </summary>
        private IEnumerator AnimateButton()
        {
            // 等待更長的時間
            yield return new WaitForSeconds(buttonAnimDelay);

            // 淡入動畫
            float elapsedTime = 0;
            Color startButtonColor = buttonImage.color;
            Color targetButtonColor = new Color(0.2f, 0.6f, 1f, 1f);
            Color startTextColor = buttonText.color;
            Color targetTextColor = Color.white;

            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeInDuration);
                buttonImage.color = Color.Lerp(startButtonColor, targetButtonColor, t);
                buttonText.color = Color.Lerp(startTextColor, targetTextColor, t);

                // 同時縮放 - 增加縮放範圍
                float scale = Mathf.Lerp(0.5f, 1.3f, t);
                buttonImage.transform.localScale = new Vector3(scale, scale, 1);
                yield return null;
            }

            buttonImage.color = targetButtonColor;
            buttonText.color = targetTextColor;

            // 彈回動畫
            elapsedTime = 0;
            float bounceDuration = 0.2f;

            while (elapsedTime < bounceDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / bounceDuration);
                float scale = Mathf.Lerp(1.3f, 1f, t);
                buttonImage.transform.localScale = new Vector3(scale, scale, 1);
                yield return null;
            }

            buttonImage.transform.localScale = Vector3.one;

            // 啟用按鈕
            if (button != null)
                button.interactable = true;

            // 開始按鈕脈動動畫
            StartCoroutine(PulseButton());
        }

        /// <summary>
        /// 文字脈動動畫
        /// </summary>
        private IEnumerator PulseText(TextMeshProUGUI text, float maxScale, float duration)
        {
            while (true)
            {
                // 放大
                float elapsedTime = 0;
                float halfDuration = duration / 2;

                while (elapsedTime < halfDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / halfDuration);
                    float scale = Mathf.Lerp(1f, maxScale, t);
                    text.transform.localScale = new Vector3(scale, scale, 1);
                    yield return null;
                }

                // 縮小
                elapsedTime = 0;

                while (elapsedTime < halfDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / halfDuration);
                    float scale = Mathf.Lerp(maxScale, 1f, t);
                    text.transform.localScale = new Vector3(scale, scale, 1);
                    yield return null;
                }

                text.transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// 按鈕脈動動畫
        /// </summary>
        private IEnumerator PulseButton()
        {
            while (true)
            {
                // 顏色變化
                float elapsedTime = 0;
                float halfDuration = pulseDuration / 2;
                Color startColor = new Color(0.2f, 0.6f, 1f, 1f);
                Color endColor = new Color(0.5f, 0.9f, 1f, 1f); // 增加顏色對比

                while (elapsedTime < halfDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / halfDuration);
                    buttonImage.color = Color.Lerp(startColor, endColor, t);
                    yield return null;
                }

                elapsedTime = 0;

                while (elapsedTime < halfDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / halfDuration);
                    buttonImage.color = Color.Lerp(endColor, startColor, t);
                    yield return null;
                }

                buttonImage.color = startColor;
            }
        }
    }
}
