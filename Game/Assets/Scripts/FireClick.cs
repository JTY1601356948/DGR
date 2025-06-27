using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FireClick : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // 存储原始颜色
    private Material material;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // 保存初始颜色
        material = spriteRenderer.material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 将颜色变浅（RGB各通道乘以1.2，但不超过1）
            //Color lighterColor = originalColor * 0.7f;
            //lighterColor = new Color(
            //    Mathf.Clamp(lighterColor.r, 0, 1),
            //    Mathf.Clamp(lighterColor.g, 0, 1),
            //    Mathf.Clamp(lighterColor.b, 0, 1),
            //    originalColor.a // 保持Alpha不变
            //);
            //spriteRenderer.color = lighterColor;
            
             material.EnableKeyword("_EMISSION");
             Color glowColor = new Color(1, 1, 0); // 黄色发光，可调整
             material.SetColor("_EmissionColor", glowColor);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 恢复原始颜色
            spriteRenderer.color = originalColor;
        }
    }
}