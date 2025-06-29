using System.Collections;
using UnityEngine;

public class AudioFadeController : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("将要控制的音频源")]
    public AudioSource audioSource;

    [Header("Fade Settings")]
    [Tooltip("淡出效果持续时间（秒）")]
    public float fadeDuration = 2.0f;

    [Tooltip("启用自动淡出（在音乐结束前开始）")]
    public bool autoFade = true;

    // 原始音量（用于恢复）
    private float originalVolume;

    // 是否正在淡出中
    private bool isFadingOut = false;

    void Start()
    {
        // 确保音频源存在
        if (audioSource == null)
        {
            Debug.LogWarning("未分配AudioSource，尝试查找");
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError("场景中找不到AudioSource组件！", this);
            return;
        }

        // 保存原始音量
        originalVolume = audioSource.volume;

        // 自动淡出
        if (autoFade && audioSource.clip != null)
        {
            StartAutoFade();
        }
    }

    // 开始自动淡出（在音乐结束前启动）
    public void StartAutoFade()
    {
        if (isFadingOut) return;

        if (audioSource != null && audioSource.clip != null)
        {
            float timeToStartFade = CalculateFadeStartTime();
            StartCoroutine(DelayedFadeStart(timeToStartFade));
        }
        else
        {
            Debug.LogError("无法开始自动淡出：AudioSource或音频剪辑未设置", this);
        }
    }

    // 立即开始淡出（可手动调用）
    public void StartFadeOut()
    {
        if (isFadingOut) return;
        isFadingOut = true;
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator DelayedFadeStart(float delay)
    {
        Debug.Log($"将在 {delay} 秒后开始淡出");
        yield return new WaitForSeconds(delay);
        StartFadeOut();
    }

    private IEnumerator FadeOutCoroutine()
    {
        isFadingOut = true;

        if (!audioSource.isPlaying)
        {
            Debug.LogWarning("尝试在未播放的音频源上淡出");
            yield break;
        }

        Debug.Log("淡出开始...");

        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // 计算新音量（线性变化）
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终音量为0
        audioSource.volume = 0f;

        // 停止音频源（可选）
        audioSource.Stop();

        // 恢复原音量（为了下次播放）
        audioSource.volume = originalVolume;

        Debug.Log("淡出完成");

        isFadingOut = false;
    }

    // 计算应该开始淡出的时间
    private float CalculateFadeStartTime()
    {
        float timeToStartFade = audioSource.clip.length - fadeDuration;

        // 确保淡出时间不超过音频总长度
        if (timeToStartFade < 0)
        {
            Debug.LogWarning($"淡出时间 ({fadeDuration}秒) 长于音频长度 ({audioSource.clip.length}秒)。将立即开始淡出。");
            return 0;
        }

        return timeToStartFade;
    }
}