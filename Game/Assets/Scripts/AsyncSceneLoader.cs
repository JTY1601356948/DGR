using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AsyncSceneLoader : MonoBehaviour
{
    [Header("Loading Settings")]
    public GameObject loadingScreen;          // ����UI���
    public Slider progressBar;               // ������
    public float minLoadTime = 2.0f;          // ��С����ʱ�䣨�룩
    public float progressSmoothSpeed = 3.0f;  // ������ƽ�������ٶ�
    public CanvasGroup fadeCanvasGroup;       // ���ڵ��뵭����CanvasGroup

    [Header("Fade Settings")]
    public float fadeDuration = 1.0f;         // ���뵭��ʱ�䣨�룩
    public bool fadeInOnLoad = true;          // �Ƿ����³�������ʱ�Զ�����

    private float displayedProgress;          // ��ǰ��ʾ�Ľ���ֵ������ƽ�����ɣ�

    void Start()
    {
        // ��ʼ�����뵭��Ч��
        if (fadeCanvasGroup != null && fadeInOnLoad)
        {
            fadeCanvasGroup.alpha = 1f; // ��ʼʱ��͸��
            StartCoroutine(Fade(0f));    // ���뵽͸��
        }
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        // ������ʾ����
        displayedProgress = 0f;

        // ��������˵���Ч������ִ�е���
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(1f)); // ��������͸��
        }

        // ��¼��ʼ���ص�ʱ��
        float startTime = Time.realtimeSinceStartup;

        // ��ʾ���ؽ���
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // ���ý�����
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }

        // ��ʼ�첽���س���
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // �����Զ������
        operation.allowSceneActivation = false;

        // ʵ�ʼ��ؽ���
        float realProgress = 0f;

        // ������ɱ��
        bool reached90Percent = false;

        // ����ѭ��
        while (!operation.isDone || displayedProgress < 0.999f)
        {
            // ����ʵ�ʽ��ȣ�0-0.9��Χ��
            realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // ʹ��Lerpʵ�ֽ�����ƽ������
            displayedProgress = Mathf.Lerp(
                displayedProgress,
                realProgress,
                progressSmoothSpeed * Time.deltaTime
            );

            // ȷ����ʾ���Ȳ�С����ʵ����
            if (displayedProgress < realProgress)
            {
                displayedProgress = realProgress;
            }

            // ���½�������ʾ
            if (progressBar != null)
            {
                progressBar.value = displayedProgress;
            }

            // ���ȴﵽ90%��
            if (operation.progress >= 0.9f && !reached90Percent)
            {
                reached90Percent = true;

                // �����Ѿ����ص�ʱ��
                float elapsedTime = Time.realtimeSinceStartup - startTime;

                // ������Ҫ����ȴ���ʱ����ȷ���ﵽ��С����ʱ��
                float extraWaitTime = minLoadTime - elapsedTime;

                // �����Ҫ����ȴ�
                if (extraWaitTime > 0)
                {
                    float waitStartTime = Time.realtimeSinceStartup;

                    // �ڵȴ��ڼ����ƽ�����ȵ�100%
                    while (Time.realtimeSinceStartup - waitStartTime < extraWaitTime)
                    {
                        // ƽ�����ɵ�100%
                        displayedProgress = Mathf.Lerp(displayedProgress, 1f, progressSmoothSpeed * Time.deltaTime);
                        if (progressBar != null) progressBar.value = displayedProgress;
                        yield return null;
                    }
                }

                // ȷ��������ʾΪ100%
                while (displayedProgress < 0.999f)
                {
                    displayedProgress = Mathf.Lerp(displayedProgress, 1f, progressSmoothSpeed * Time.deltaTime);
                    if (progressBar != null) progressBar.value = displayedProgress;
                    yield return null;
                }

                if (progressBar != null) progressBar.value = 1f;

                // �ڼ����ǰ�ȴ�һ�����չʾ�����Ľ�������
                yield return new WaitForSeconds(0.3f);

                // ��������˵���Ч������ִ�е��뵽��͸��
                if (fadeCanvasGroup != null)
                {
                    yield return StartCoroutine(Fade(1f)); // ȷ����ȫ��͸��
                }

                // �ֶ������
                operation.allowSceneActivation = true;

                // �ȴ�������ȫ����
                yield return null;

                // ���³����е�����͸��
                if (fadeInOnLoad && fadeCanvasGroup != null)
                {
                    yield return StartCoroutine(Fade(0f));
                }
            }

            yield return null;
        }

        // ���ؼ��ؽ���
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    // ���뵭��Э��
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