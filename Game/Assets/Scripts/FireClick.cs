using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FireClick : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor; // �洢ԭʼ��ɫ

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // �����ʼ��ɫ
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ����ɫ��ǳ��RGB��ͨ������1.2����������1��
            Color lighterColor = originalColor * 0.7f;
            lighterColor = new Color(
                Mathf.Clamp(lighterColor.r, 0, 1),
                Mathf.Clamp(lighterColor.g, 0, 1),
                Mathf.Clamp(lighterColor.b, 0, 1),
                originalColor.a // ����Alpha����
            );
            spriteRenderer.color = lighterColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �ָ�ԭʼ��ɫ
            spriteRenderer.color = originalColor;
        }
    }
}