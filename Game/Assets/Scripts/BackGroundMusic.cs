using UnityEngine;

public class SimpleBackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // ������Ч������Unity�༭���и�ֵ
    public float volume = 0.5f;       // ������С

    private AudioSource audioSource;

    void Awake()
    {
        // �����õ������л�����ʱ�Զ�����
        SetupAudioSource();
    }

    void SetupAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = true;  // ѭ������
        audioSource.playOnAwake = true; // �������غ��Զ�����
    }

    // ��ѡ����Start�м���Ƿ��Ѳ��ţ�ȷ��ֻ����һ�Σ�
    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}