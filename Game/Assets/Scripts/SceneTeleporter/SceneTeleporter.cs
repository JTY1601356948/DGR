using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // �Ƿ��ͽӿ�����[1](@ref)
using System.Collections.Generic; // ���ͽӿ��貹��[5](@ref)

public class SceneTeleporter : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private float fadeDuration = 1f;

    [Header("����Ԫ��")]
    [SerializeField] private CanvasGroup fadeCanvas;

    private bool canTeleport = false;

    // ���봥������ʱ��ǿɴ���
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = true;
            Debug.Log("���봫�����򣬰�Tab�л�����");
        }
    }

    // �뿪����ʱȡ�����
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
        // ����Ч��
        yield return StartCoroutine(FadeScreen(0, 1));

        // �첽���س���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false;

        // �ȴ����ؽ��ȴﵽ90%
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // �����³���������
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