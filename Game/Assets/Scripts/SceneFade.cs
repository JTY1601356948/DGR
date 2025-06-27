using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        // 确保初始为纯黑
        fadeImage.color = Color.black;
        StartCoroutine(FadeEffect());
    }

    private System.Collections.IEnumerator FadeEffect()
    {
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // 完成后禁用组件
        fadeImage.gameObject.SetActive(false);
    }
}