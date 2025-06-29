using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum onButton
{
    ZhongJi,
    QinJi
}
public class GameController : MonoBehaviour
{
    // TimeLog ��ر���
    [Header("ʱ����ʾ")]
    public TMP_Text dialogText;
    public TMP_Text shichen;
    public int minutes = 45;
    public int hours = 18;
    private string shiKe;
    float accumulator = 0f;
    int chance = 0;

    // ButtonClick ��ر���
    [Header("��ť�͵���")]
    public Button ZhongJi;
    public Button QinJi;
    public GameObject Win;
    public GameObject Lose;
    public GameObject tiShiLuo;
    public GameObject tiShi2;
    public GameObject tiShi3;
    public GameObject tiShi4;
    public GameObject tiShi5;
    public GameObject GameOverPanel;
    public GameObject GameOverLose;
    public GameObject GameOverWin;

    [Header("��Ϸ�߼�")]
    private List<onButton> playerSequence = new List<onButton>();
    private float resetTimer = 0f;
    private bool isResetting = false;
    private int winNum = 0;
    private onButton[] LuoGen = new onButton[] { onButton.ZhongJi, onButton.QinJi, onButton.ZhongJi ,onButton.QinJi};
    private onButton[] Gen2 = new onButton[] { onButton.QinJi, onButton.QinJi, onButton.QinJi, onButton.QinJi ,onButton.QinJi};
    private onButton[] Gen3 = new onButton[] { onButton.ZhongJi, onButton.QinJi, onButton.QinJi };
    private onButton[] Gen4 = new onButton[] { onButton.ZhongJi, onButton.QinJi, onButton.QinJi, onButton.QinJi };
    private onButton[] Gen5 = new onButton[] { onButton.ZhongJi, onButton.QinJi, onButton.QinJi, onButton.QinJi ,onButton.QinJi};


    
    void Start()
    {
        // ��ʼ����ť�¼�
        ZhongJi.onClick.AddListener(OnButtonZhong);
        QinJi.onClick.AddListener(OnButtonQin);

        // ��ʼ������
        Win.SetActive(false);
        Lose.SetActive(false);
        tiShiLuo.SetActive(false);
        tiShi2.SetActive(false);
        tiShi3.SetActive(false);
        tiShi4.SetActive(false);
        tiShi5.SetActive(false);

    }

    void Update()
    {
        // ʱ������߼�
        NowTime();
        Shike();
        UpdateTimeDisplay();

        // ��ť��֤�߼�
        ValidateSequence();
        HandleResetTimer();
        chanceCheck();
    }

    void NowTime()
    {
        accumulator += Time.deltaTime * 3;
        int newMinutes = (45+(int)accumulator) % 60;
        int newHours = 18 + ((int)accumulator+45) / 60;
        if (newHours >= 24) newHours -= 24;

        hours = newHours;
        minutes = newMinutes;
    }

    //void NowTime()
    //{
    //    int intTime = (int)(Time.time)*3;
    //    hours = 18 + (intTime + 45) / 60;
    //    if (hours >= 24) hours -= 24;
    //    minutes = (45 + intTime) % 60;
    //}

    void chanceCheck()
    {
        float totalMinutes = hours * 60 + minutes;
        if (chance<1&&totalMinutes>1155)
        {
            GameOverPanel.SetActive(true);
            GameOverLose.SetActive(true);
        }
        if (chance < 2 && totalMinutes > 1275)
        {
            GameOverPanel.SetActive(true);
            GameOverLose.SetActive(true);
        }
        if (chance < 3 && totalMinutes > 1395)
        {
            GameOverPanel.SetActive(true);
            GameOverLose.SetActive(true);
        }
        if (chance < 4 && totalMinutes > 1515)
        {
            GameOverPanel.SetActive(true);
            GameOverLose.SetActive(true);
        }
        if (chance < 5 && totalMinutes > 1635)
        {
            GameOverPanel.SetActive(true);
            GameOverLose.SetActive(true);
        }
        if(chance>=5&&totalMinutes>1635)
        {
            GameOverPanel.SetActive(true);
            GameOverWin.SetActive(true);

        }
    }

    void Shike()
    {
        switch (hours)
        {
            case int h when h >= 19 && h < 21: shiKe = "��ʱ"; break;
            case int h when h >= 21 && h < 23: shiKe = "��ʱ"; break;
            case int h when h >= 23 || h < 1: shiKe = "��ʱ"; break;
            case int h when h >= 1 && h < 3: shiKe = "��ʱ"; break;
            case int h when h >= 3 && h < 5: shiKe = "��ʱ"; break;
            case int h when h >= 5 && h < 7: shiKe = "îʱ"; break;
            case int h when h >= 7 && h < 9: shiKe = "��ʱ"; break;
            case int h when h >= 9 && h < 11: shiKe = "��ʱ"; break;
            case int h when h >= 11 && h < 13: shiKe = "��ʱ"; break;
            case int h when h >= 13 && h < 15: shiKe = "δʱ"; break;
            case int h when h >= 15 && h < 17: shiKe = "��ʱ"; break;
            case int h when h >= 17 && h < 19: shiKe = "��ʱ"; break;
            default: shiKe = "δ֪ʱ��"; break;
        }
    }

    void UpdateTimeDisplay()
    {
        dialogText.text = "��ʱ��" + hours.ToString("D2") + ":" + minutes.ToString("D2");
        shichen.text = "ʱ����" + shiKe;
    }

    void ValidateSequence()
    {
        float totalMinutes = hours * 60 + minutes;
        bool isTimeInRangeLuo = totalMinutes >= 1125 && totalMinutes <= 1155; // 18:45 ~ 19:15
        bool isTimeInRange2 = totalMinutes >= 1245 && totalMinutes <= 1275;
        bool isTimeInRange3 = totalMinutes >= 1365 && totalMinutes <= 1395;
        bool isTimeInRange4 = totalMinutes >= 1485 && totalMinutes <= 1515;
        bool isTimeInRange5 = totalMinutes >= 1605 && totalMinutes <= 1635;

        tiShiLuo.SetActive(isTimeInRangeLuo);
        tiShi2.SetActive(isTimeInRange2);
        tiShi3.SetActive(isTimeInRange3);
        tiShi4.SetActive(isTimeInRange4);
        tiShi5.SetActive(isTimeInRange5);


        if (isTimeInRangeLuo && playerSequence.Count == LuoGen.Length)//111
        {
            bool isCorrect = true;
            for (int i = 0; i < LuoGen.Length; i++)
            {
                if (playerSequence[i] != LuoGen[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                Win.SetActive(true);
                chance += 1;
                if(chance>1)
                    { chance = 1; }
                isResetting = true;
            }
            else
            {
                Lose.SetActive(true);
                isResetting = true;
            }
        }
        if (isTimeInRange2 && playerSequence.Count == Gen2.Length)//22222
        {
            bool isCorrect = true;
            for (int i = 0; i < Gen2.Length; i++)
            {
                if (playerSequence[i] != Gen2[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                Win.SetActive(true);
                chance += 1;
                if (chance > 2)
                { chance = 2; }
                isResetting = true;
            }
            else
            {
                Lose.SetActive(true);
                isResetting = true;
            }
        }
        if (isTimeInRange3&& playerSequence.Count == Gen3.Length)
        {
            bool isCorrect = true;
            for (int i = 0; i < Gen3.Length; i++)
            {
                if (playerSequence[i] != Gen3[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                Win.SetActive(true);
                chance += 1;
                if (chance > 3)
                { chance = 3; }
                isResetting = true;
            }
            else
            {
                Lose.SetActive(true);
                isResetting = true;
            }
        }
        if (isTimeInRange4 && playerSequence.Count == Gen4.Length)
        {
            bool isCorrect = true;
            for (int i = 0; i < Gen4.Length; i++)
            {
                if (playerSequence[i] != Gen4[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                Win.SetActive(true);
                chance += 1;
                if (chance > 4)
                { chance = 4; }
                isResetting = true;
            }
            else
            {
                Lose.SetActive(true);
                isResetting = true;
            }
        }
        if (isTimeInRange5 && playerSequence.Count == Gen5.Length)
        {
            bool isCorrect = true;
            for (int i = 0; i < Gen5.Length; i++)
            {
                if (playerSequence[i] != Gen5[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                Win.SetActive(true);
                chance += 1;
                if (chance > 5)
                { chance = 5; }
                isResetting = true;
            }
            else
            {
                Lose.SetActive(true);
                isResetting = true;
            }
        }
    }


    void HandleResetTimer()
    {
        if (isResetting)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer >= 2f)
            {
                ResetGame();
                isResetting = false;
                resetTimer = 0f;
            }
        }
    }

    void ResetGame()
    {
        playerSequence.Clear();
        Win.SetActive(false);
        Lose.SetActive(false);
        tiShiLuo.SetActive(false);
        tiShi2.SetActive(false);
        tiShi3.SetActive(false);
        tiShi4.SetActive(false);
        tiShi5.SetActive(false);

    }

    // ��ť�¼�����
    void OnButtonZhong() => HandleButtonClick(onButton.ZhongJi);
    void OnButtonQin() => HandleButtonClick(onButton.QinJi);

    void HandleButtonClick(onButton button)
    {
        playerSequence.Add(button);
        Debug.Log("��ǰ��������: " + string.Join(", ", playerSequence));
    }
}
