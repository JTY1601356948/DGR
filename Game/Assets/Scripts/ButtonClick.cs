using System;
using UnityEngine;
using UnityEngine.UI;
public class ButtonClick : MonoBehaviour
{
    public Button ZhongJi; // ��ק��ť�� Inspector
    public Button QinJi;

    void Start()
    {
        // ��̬�󶨵���¼�
        ZhongJi.onClick.AddListener(OnButtonZhong);
        QinJi.onClick.AddListener(OnButtonQin);

    }


    void OnButtonZhong()
    {
        Debug.Log("�ػ���");
    }
    void OnButtonQin()
    {
        Debug.Log("���!");
    }
}
