using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // 添加这个命名空间引用

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeOverlay; // 黑色遮罩对象
    [SerializeField] private float fadeDuration = 1.0f; // 淡入淡出持续时间

    public static SceneTransitionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 确保覆盖层在场景切换时保持不变
        if (fadeOverlay != null)
        {
            DontDestroyOnLoad(fadeOverlay.transform.parent.gameObject);
        }

        // 初始设置为完全透明
        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0, 0, 0, 0);
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncWithFade(sceneName));
    }

    private IEnumerator LoadSceneAsyncWithFade(string sceneName)
    {
        // 淡出到黑屏
        if (fadeOverlay != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // 异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true; // 确保场景可以激活

        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 淡入恢复显示
        if (fadeOverlay != null)
        {
            yield return StartCoroutine(FadeFromBlack());
        }

        // 可选：加载完成后销毁管理器（根据需求）
        // Destroy(gameObject);
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color startColor = fadeOverlay.color;
        Color targetColor = new Color(0, 0, 0, 1);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeOverlay.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeOverlay.color = targetColor;
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        Color startColor = fadeOverlay.color;
        Color targetColor = new Color(0, 0, 0, 0);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeOverlay.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeOverlay.color = targetColor;
    }

    // 新增：直接控制淡入淡出的公共方法
    public void FadeOut(System.Action onComplete = null)
    {
        StartCoroutine(FadeToBlackRoutine(onComplete));
    }

    public void FadeIn(System.Action onComplete = null)
    {
        StartCoroutine(FadeFromBlackRoutine(onComplete));
    }

    private IEnumerator FadeToBlackRoutine(System.Action onComplete)
    {
        yield return FadeToBlack();
        onComplete?.Invoke();
    }

    private IEnumerator FadeFromBlackRoutine(System.Action onComplete)
    {
        yield return FadeFromBlack();
        onComplete?.Invoke();
    }
}