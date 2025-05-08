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
        if (hours >= 19 && hours < 21)
            shiKe = "戌时";
        else if (hours >= 21 && hours < 23)
            shiKe = "亥时";
        else if (hours >= 23 || hours < 1)
            shiKe = "子时";
        else if (hours >= 1 && hours < 3)
            shiKe = "丑时";
        else if (hours >= 3 && hours < 5)
            shiKe = "寅时";
        else if (hours >= 5 && hours < 7)
            shiKe = "卯时​";
        else if (hours >= 7 && hours < 9)
            shiKe = "​​辰时​​";
        else if (hours >= 9 && hours < 11)
            shiKe = "巳时​";
        else if (hours >= 11 && hours < 13)
            shiKe = "​​午时​​";
        else if (hours >= 13 && hours < 15)
            shiKe = "​​未时​";
        else if (hours >= 15 && hours < 17)
            shiKe = "​​申时​";
        else if (hours >= 17 && hours < 19)
            shiKe = "酉时​";

    }
}
