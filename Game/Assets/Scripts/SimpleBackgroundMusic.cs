using UnityEngine;
using System.Collections;

public class SimpleBackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // ������Ч
    public float volume = 0.5f;       // ������С
    public float fadeDuration = 1.0f; // ��������ʱ��

    private AudioSource audioSource;
    private bool isFading = false;    // ����Ƿ����ڵ���
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

    // �������������ɰ�ť����
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

        // ������ɺ�������Ϸ����
        Destroy(gameObject);
    }

    // ��ֹ��Ӧ���˳�ʱ��������Э��
    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    // �����л�ʱ�Ĵ���
    void OnDestroy()
    {
        if (isApplicationQuitting || isFading) return;

        // ������屻���ٵ�δ���ڵ��������У���ȫ����������Э��
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }
}