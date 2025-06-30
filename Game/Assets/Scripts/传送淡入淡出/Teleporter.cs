using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [Header("传送设置")]
    [SerializeField] Transform destination; // 目标传送位置
    [SerializeField] float fadeDuration = 0.5f; // 淡入淡出持续时间
    [SerializeField] float cooldownTime = 2f; // 传送冷却时间

    [Header("参考对象")]
    [SerializeField] Image fadeImage; // 黑色遮罩图像 (UI Image组件)

    private bool isTeleporting = false; // 防止重复触发
    private float cooldownEndTime; // 冷却结束时间

    // 当前传送器是否在冷却中
    public bool OnCooldown => Time.time < cooldownEndTime;

    void Start()
    {
        // 确保有fadeImage引用
        if (fadeImage == null)
        {
            CreateFadeImage();
        }

        // 初始状态透明
        SetFadeAlpha(0);
    }

    // 自动创建遮罩（如果未赋值）
    void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeOverlay");
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // 最高层级

        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = false; // 不阻挡点击事件

        RectTransform rt = fadeObject.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 正在传送中或冷却中时忽略触发
        if (isTeleporting || OnCooldown) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportSequence(other.transform));
        }
    }

    IEnumerator TeleportSequence(Transform player)
    {
        isTeleporting = true;

        // 让当前传送器进入冷却
        ActivateCooldown();

        // 让目标传送器也进入冷却
        Teleporter targetTeleporter = destination.GetComponent<Teleporter>();
        if (targetTeleporter != null)
        {
            targetTeleporter.ActivateCooldown();
        }

        // 淡入效果
        yield return StartCoroutine(FadeScreen(0, 1));

        // 执行传送
        player.position = destination.position;

        // 淡出效果
        yield return StartCoroutine(FadeScreen(1, 0));

        isTeleporting = false;
    }

    // 启动冷却计时器
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