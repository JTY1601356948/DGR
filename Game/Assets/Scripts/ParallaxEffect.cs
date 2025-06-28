// ParallaxEffect.cs
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // 该物体的移动系数（0-1）
    // 1 = 与摄像机同步移动，>1 = 比摄像机移动更快，<1 = 比摄像机移动更慢
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
        // 计算摄像机移动的差值
        float parallaxX = (previousCamPos.x - cam.position.x) * parallaxEffect;
        float parallaxY = (previousCamPos.y - cam.position.y) * parallaxEffect * 0.5f; // Y轴移动量为X轴一半，使效果更自然

        // 应用视差移动
        transform.position += new Vector3(parallaxX, parallaxY, 0);

        // 更新摄像机位置记录
        previousCamPos = cam.position;
    }
}