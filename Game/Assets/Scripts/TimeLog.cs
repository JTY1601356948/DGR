using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeLog : MonoBehaviour
{

    public TMP_Text dialogText;
    public TMP_Text shichen;
    public int minutes=45;
    public int hours=18;
    private string shiKe;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nowTime();
        shike();
        dialogText.text = "更时："+hours.ToString("D2")+":"+minutes.ToString("D2");
        shichen.text = "时辰：" + shiKe.ToString();
    }

    void nowTime()
    {
        int intTime=(int)(Time.time);
        hours =18+ ( intTime+ 45) / 60;
        if (hours >= 24)
            hours -= 24;
        minutes = (45 + intTime) % 60;
    }

    void shike()
    {
        switch (hours)
        {
            case int h when h >= 19 && h < 21:
                shiKe = "戌时";
                break;
            case int h when h >= 21 && h < 23:
                shiKe = "亥时";
                break;
            case int h when h >= 23 || h < 1:
                shiKe = "子时";
                break;
            case int h when h >= 1 && h < 3:
                shiKe = "丑时";
                break;
            case int h when h >= 3 && h < 5:
                shiKe = "寅时";
                break;
            case int h when h >= 5 && h < 7:
                shiKe = "卯时";
                break;
            case int h when h >= 7 && h < 9:
                shiKe = "辰时";
                break;
            case int h when h >= 9 && h < 11:
                shiKe = "巳时";
                break;
            case int h when h >= 11 && h < 13:
                shiKe = "午时";
                break;
            case int h when h >= 13 && h < 15:
                shiKe = "未时";
                break;
            case int h when h >= 15 && h < 17:
                shiKe = "申时";
                break;
            case int h when h >= 17 && h < 19:
                shiKe = "酉时";
                break;
            default:
                shiKe = "未知时辰";
                break;
        }
    }
}
