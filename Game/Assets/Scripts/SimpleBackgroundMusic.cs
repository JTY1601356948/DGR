using UnityEngine;
using System.Collections;

public class SimpleBackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // 背景音效
    public float volume = 0.5f;       // 音量大小
    public float fadeDuration = 1.0f; // 淡出持续时间

    private AudioSource audioSource;
    private bool isFading = false;    // 标记是否正在淡出
    private static bool isApplicationQuitting = false;

    void Awake()
    {
        SetupAudioSource();
    }

    void SetupAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
    }

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // 公共方法：可由按钮触发
    public void FadeOutAndDestroy()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutCoroutine());
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        // 淡出完成后销毁游戏对象
        Destroy(gameObject);
    }

    // 防止在应用退出时触发淡出协程
    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    // 场景切换时的处理
    void OnDestroy()
    {
        if (isApplicationQuitting || isFading) return;

        // 如果物体被销毁但未处于淡出过程中，安全地启动淡出协程
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }
}