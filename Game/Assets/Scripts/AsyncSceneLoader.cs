using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AsyncSceneLoader : MonoBehaviour
{
    [Header("Loading Settings")]
    public GameObject loadingScreen;          // 加载UI面板
    public Slider progressBar;               // 进度条
    public float minLoadTime = 2.0f;          // 最小加载时间（秒）
    public float progressSmoothSpeed = 3.0f;  // 进度条平滑过渡速度
    public CanvasGroup fadeCanvasGroup;       // 用于淡入淡出的CanvasGroup

    [Header("Fade Settings")]
    public float fadeDuration = 1.0f;         // 淡入淡出时间（秒）
    public bool fadeInOnLoad = true;          // 是否在新场景加载时自动淡入

    private float displayedProgress;          // 当前显示的进度值（用于平滑过渡）

    void Start()
    {
        // 初始化淡入淡出效果
        if (fadeCanvasGroup != null && fadeInOnLoad)
        {
            fadeCanvasGroup.alpha = 1f; // 开始时不透明
            StartCoroutine(Fade(0f));    // 淡入到透明
        }
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        // 重置显示进度
        displayedProgress = 0f;

        // 如果设置了淡出效果，先执行淡出
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(1f)); // 淡出到不透明
        }

        // 记录开始加载的时间
        float startTime = Time.realtimeSinceStartup;

        // 显示加载界面
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // 重置进度条
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }

        // 开始异步加载场景
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 禁用自动激活场景
        operation.allowSceneActivation = false;

        // 实际加载进度
        float realProgress = 0f;

        // 加载完成标记
        bool reached90Percent = false;

        // 加载循环
        while (!operation.isDone || displayedProgress < 0.999f)
        {
            // 计算实际进度（0-0.9范围）
            realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // 使用Lerp实现进度条平滑过渡
            displayedProgress = Mathf.Lerp(
                displayedProgress,
                realProgress,
                progressSmoothSpeed * Time.deltaTime
            );

            // 确保显示进度不小于真实进度
            if (displayedProgress < realProgress)
            {
                displayedProgress = realProgress;
            }

            // 更新进度条显示
            if (progressBar != null)
            {
                progressBar.value = displayedProgress;
            }

            // 进度达到90%后
            if (operation.progress >= 0.9f && !reached90Percent)
            {
                reached90Percent = true;

                // 计算已经加载的时间
                float elapsedTime = Time.realtimeSinceStartup - startTime;

                // 计算需要额外等待的时间以确保达到最小加载时间
                float extraWaitTime = minLoadTime - elapsedTime;

                // 如果需要额外等待
                if (extraWaitTime > 0)
                {
                    float waitStartTime = Time.realtimeSinceStartup;

                    // 在等待期间继续平滑进度到100%
                    while (Time.realtimeSinceStartup - waitStartTime < extraWaitTime)
                    {
                        // 平滑过渡到100%
                        displayedProgress = Mathf.Lerp(displayedProgress, 1f, progressSmoothSpeed * Time.deltaTime);
                        if (progressBar != null) progressBar.value = displayedProgress;
                        yield return null;
                    }
                }

                // 确保进度显示为100%
                while (displayedProgress < 0.999f)
                {
                    displayedProgress = Mathf.Lerp(displayedProgress, 1f, progressSmoothSpeed * Time.deltaTime);
                    if (progressBar != null) progressBar.value = displayedProgress;
                    yield return null;
                }

                if (progressBar != null) progressBar.value = 1f;

                // 在激活场景前等待一会儿（展示完整的进度条）
                yield return new WaitForSeconds(0.3f);

                // 如果设置了淡入效果，先执行淡入到不透明
                if (fadeCanvasGroup != null)
                {
                    yield return StartCoroutine(Fade(1f)); // 确保完全不透明
                }

                // 手动激活场景
                operation.allowSceneActivation = true;

                // 等待场景完全激活
                yield return null;

                // 在新场景中淡出到透明
                if (fadeInOnLoad && fadeCanvasGroup != null)
                {
                    yield return StartCoroutine(Fade(0f));
                }
            }

            yield return null;
        }

        // 隐藏加载界面
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    // 淡入淡出协程
    IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}