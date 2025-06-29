using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // �����������ռ�����

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeOverlay; // ��ɫ���ֶ���
    [SerializeField] private float fadeDuration = 1.0f; // ���뵭������ʱ��

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

        // ȷ�����ǲ��ڳ����л�ʱ���ֲ���
        if (fadeOverlay != null)
        {
            DontDestroyOnLoad(fadeOverlay.transform.parent.gameObject);
        }

        // ��ʼ����Ϊ��ȫ͸��
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
        // ����������
        if (fadeOverlay != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // �첽���س���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true; // ȷ���������Լ���

        // �ȴ������������
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ����ָ���ʾ
        if (fadeOverlay != null)
        {
            yield return StartCoroutine(FadeFromBlack());
        }

        // ��ѡ��������ɺ����ٹ���������������
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

    // ������ֱ�ӿ��Ƶ��뵭���Ĺ�������
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