using UnityEngine;

public class MoonRotate : MonoBehaviour
{
    // ��ת�ٶȣ���/�룩
    public float rotationSpeed = 180f;

    void Update()
    {
        // �� Z ����ת��2D �����ı�׼��ת�ᣩ
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        //Debug.Log(Time.time);
    }
}
