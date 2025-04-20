using UnityEngine;

public class MoonRotate : MonoBehaviour
{
    // 旋转速度（度/秒）
    public float rotationSpeed = 180f;

    void Update()
    {
        // 绕 Z 轴旋转（2D 场景的标准旋转轴）
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        //Debug.Log(Time.time);
    }
}
