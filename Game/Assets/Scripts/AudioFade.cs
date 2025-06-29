using System.Collections;
using UnityEngine;

public class AudioFadeController : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("��Ҫ���Ƶ���ƵԴ")]
    public AudioSource audioSource;

    [Header("Fade Settings")]
    [Tooltip("����Ч������ʱ�䣨�룩")]
    public float fadeDuration = 2.0f;

    [Tooltip("�����Զ������������ֽ���ǰ��ʼ��")]
    public bool autoFade = true;

    // ԭʼ���������ڻָ���
    private float originalVolume;

    // �Ƿ����ڵ�����
    private bool isFadingOut = false;

    void Start()
    {
        // ȷ����ƵԴ����
        if (audioSource == null)
        {
            Debug.LogWarning("δ����AudioSource�����Բ���");
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError("�������Ҳ���AudioSource�����", this);
            return;
        }

        // ����ԭʼ����
        originalVolume = audioSource.volume;

        // �Զ�����
        if (autoFade && audioSource.clip != null)
        {
            StartAutoFade();
        }
    }

    // ��ʼ�Զ������������ֽ���ǰ������
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
            Debug.LogError("�޷���ʼ�Զ�������AudioSource����Ƶ����δ����", this);
        }
    }

    // ������ʼ���������ֶ����ã�
    public void StartFadeOut()
    {
        if (isFadingOut) return;
        isFadingOut = true;
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator DelayedFadeStart(float delay)
    {
        Debug.Log($"���� {delay} ���ʼ����");
        yield return new WaitForSeconds(delay);
        StartFadeOut();
    }

    private IEnumerator FadeOutCoroutine()
    {
        isFadingOut = true;

        if (!audioSource.isPlaying)
        {
            Debug.LogWarning("������δ���ŵ���ƵԴ�ϵ���");
            yield break;
        }

        Debug.Log("������ʼ...");

        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // ���������������Ա仯��
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ����������Ϊ0
        audioSource.volume = 0f;

        // ֹͣ��ƵԴ����ѡ��
        audioSource.Stop();

        // �ָ�ԭ������Ϊ���´β��ţ�
        audioSource.volume = originalVolume;

        Debug.Log("�������");

        isFadingOut = false;
    }

    // ����Ӧ�ÿ�ʼ������ʱ��
    private float CalculateFadeStartTime()
    {
        float timeToStartFade = audioSource.clip.length - fadeDuration;

        // ȷ������ʱ�䲻������Ƶ�ܳ���
        if (timeToStartFade < 0)
        {
            Debug.LogWarning($"����ʱ�� ({fadeDuration}��) ������Ƶ���� ({audioSource.clip.length}��)����������ʼ������");
            return 0;
        }

        return timeToStartFade;
    }
}