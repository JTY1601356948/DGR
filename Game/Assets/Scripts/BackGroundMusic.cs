using UnityEngine;

public class SimpleBackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // 背景音效，需在Unity编辑器中赋值
    public float volume = 0.5f;       // 音量大小

    private AudioSource audioSource;

    void Awake()
    {
        // 不设置单例，切换场景时自动销毁
        SetupAudioSource();
    }

    void SetupAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = true;  // 循环播放
        audioSource.playOnAwake = true; // 场景加载后自动播放
    }

    // 可选：在Start中检查是否已播放（确保只播放一次）
    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}