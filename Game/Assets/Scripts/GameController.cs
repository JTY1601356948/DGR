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

    // ButtonClick ��ر���
    [Header("��ť�͵���")]
    public Button ZhongJi;
    public Button QinJi;
    public GameObject Win;
    public GameObject Lose;
    public GameObject tiShiLuo;

    [Header("��Ϸ�߼�")]
    private List<onButton> playerSequence = new List<onButton>();
    private float resetTimer = 0f;
    private bool isResetting = false;
    private int winNum = 0;
    private onButton[] LuoGen = new onButton[] { onButton.ZhongJi, onButton.QinJi, onButton.ZhongJi };

    void Start()
    {
        // ��ʼ����ť�¼�
        ZhongJi.onClick.AddListener(OnButtonZhong);
        QinJi.onClick.AddListener(OnButtonQin);

        // ��ʼ������
        Win.SetActive(false);
        Lose.SetActive(false);
        tiShiLuo.SetActive(false);
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
    }

    void NowTime()
    {
        int intTime = (int)Time.time;
        hours = 18 + (intTime + 45) / 60;
        if (hours >= 24) hours -= 24;
        minutes = (45 + intTime) % 60;
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
        bool isTimeInRange = totalMinutes >= 1125 && totalMinutes <= 1155; // 18:45 ~ 19:15

        tiShiLuo.SetActive(isTimeInRange);

        if (isTimeInRange && playerSequence.Count == LuoGen.Length)
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
