// ParallaxEffect.cs
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // ��������ƶ�ϵ����0-1��
    // 1 = �������ͬ���ƶ���>1 = ��������ƶ����죬<1 = ��������ƶ�����
    public float parallaxEffect = 0.5f;

    private Transform cam;
    private Vector3 previousCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void FixedUpdate()
    {
        // ����������ƶ��Ĳ�ֵ
        float parallaxX = (previousCamPos.x - cam.position.x) * parallaxEffect;
        float parallaxY = (previousCamPos.y - cam.position.y) * parallaxEffect * 0.5f; // Y���ƶ���ΪX��һ�룬ʹЧ������Ȼ

        // Ӧ���Ӳ��ƶ�
        transform.position += new Vector3(parallaxX, parallaxY, 0);

        // ���������λ�ü�¼
        previousCamPos = cam.position;
    }
}