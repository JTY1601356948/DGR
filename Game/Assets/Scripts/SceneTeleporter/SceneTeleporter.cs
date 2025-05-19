using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // 非泛型接口所需[1](@ref)
using System.Collections.Generic; // 泛型接口需补充[5](@ref)

public class SceneTeleporter : MonoBehaviour
{
    [Header("场景设置")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private float fadeDuration = 1f;

    [Header("过渡元素")]
    [SerializeField] private CanvasGroup fadeCanvas;

    private bool canTeleport = false;

    // 进入触发区域时标记可传送
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = true;
            Debug.Log("进入传送区域，按Tab切换场景");
        }
    }

    // 离开区域时取消标记
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = false;
        }
    }

    void Update()
    {
        if (canTeleport && Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(TransitionScene());
        }
    }

    IEnumerator TransitionScene()
    {
        // 淡出效果
        yield return StartCoroutine(FadeScreen(0, 1));

        // 异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false;

        // 等待加载进度达到90%
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // 激活新场景并淡入
        asyncLoad.allowSceneActivation = true;
        yield return StartCoroutine(FadeScreen(1, 0));
    }

    IEnumerator FadeScreen(float startAlpha, float targetAlpha)
    {
        float elapsed = 0;
        fadeCanvas.alpha = startAlpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = targetAlpha;
    }

    [SerializeField] private AudioSource bgmSource;
    IEnumerator FadeAudio(float targetVolume)
    {
        float startVolume = bgmSource.volume;
        while (Mathf.Abs(startVolume - targetVolume) > 0.01f)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, Time.deltaTime / fadeDuration);
            yield return null;
        }
    }


}