using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] Transform destination; // Ŀ�괫��λ��
    [SerializeField] float fadeDuration = 0.5f; // ���뵭������ʱ��
    [SerializeField] float cooldownTime = 2f; // ������ȴʱ��

    [Header("�ο�����")]
    [SerializeField] Image fadeImage; // ��ɫ����ͼ�� (UI Image���)

    private bool isTeleporting = false; // ��ֹ�ظ�����
    private float cooldownEndTime; // ��ȴ����ʱ��

    // ��ǰ�������Ƿ�����ȴ��
    public bool OnCooldown => Time.time < cooldownEndTime;

    void Start()
    {
        // ȷ����fadeImage����
        if (fadeImage == null)
        {
            CreateFadeImage();
        }

        // ��ʼ״̬͸��
        SetFadeAlpha(0);
    }

    // �Զ��������֣����δ��ֵ��
    void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeOverlay");
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // ��߲㼶

        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = false; // ���赲����¼�

        RectTransform rt = fadeObject.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���ڴ����л���ȴ��ʱ���Դ���
        if (isTeleporting || OnCooldown) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportSequence(other.transform));
        }
    }

    IEnumerator TeleportSequence(Transform player)
    {
        isTeleporting = true;

        // �õ�ǰ������������ȴ
        ActivateCooldown();

        // ��Ŀ�괫����Ҳ������ȴ
        Teleporter targetTeleporter = destination.GetComponent<Teleporter>();
        if (targetTeleporter != null)
        {
            targetTeleporter.ActivateCooldown();
        }

        // ����Ч��
        yield return StartCoroutine(FadeScreen(0, 1));

        // ִ�д���
        player.position = destination.position;

        // ����Ч��
        yield return StartCoroutine(FadeScreen(1, 0));

        isTeleporting = false;
    }

    // ������ȴ��ʱ��
    public void ActivateCooldown()
    {
        cooldownEndTime = Time.time + cooldownTime;
    }

    IEnumerator FadeScreen(float startAlpha, float targetAlpha)
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            SetFadeAlpha(alpha);
            yield return null;
        }
        SetFadeAlpha(targetAlpha);
    }

    void SetFadeAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }

   
}