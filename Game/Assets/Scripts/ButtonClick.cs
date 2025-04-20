using System;
using UnityEngine;
using UnityEngine.UI;
public class ButtonClick : MonoBehaviour
{
    public Button ZhongJi; // 拖拽按钮到 Inspector
    public Button QinJi;

    void Start()
    {
        // 动态绑定点击事件
        ZhongJi.onClick.AddListener(OnButtonZhong);
        QinJi.onClick.AddListener(OnButtonQin);

    }


    void OnButtonZhong()
    {
        Debug.Log("重击！");
    }
    void OnButtonQin()
    {
        Debug.Log("轻击!");
    }
}
