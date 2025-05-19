using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//public enum onButton
//{
//    ZhongJi,
//    QinJi
//}
public class ButtonClick : MonoBehaviour
{
    public Button ZhongJi;
    public Button QinJi;
    private TimeLog timeLog;
    private float resetTimer = 0f;
    private bool isResetting = false;
    private int winNum = 0;

    public GameObject Win;
    public GameObject Lose;
    public GameObject tiShiLuo;
    private List<onButton> playerSequence = new List<onButton>();

    public onButton[] LuoGen = new onButton[] { onButton.ZhongJi, onButton.QinJi, onButton.ZhongJi };

    void Start()
    {
        ZhongJi.onClick.AddListener(OnButtonZhong);
        QinJi.onClick.AddListener(OnButtonQin);
        Win.SetActive(false);
        Lose.SetActive(false);
        tiShiLuo.SetActive(false);
    }
    void Update()
    {
        if (isResetting)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer >= 2f) // 2 秒后重置
            {
                ResetGame();
                isResetting = false;
                resetTimer = 0f;
            }
        }
        ValidateSequence();
    }

    void OnButtonZhong()
    {
        HandleButtonClick(onButton.ZhongJi);
    }

    void OnButtonQin()
    {
        HandleButtonClick(onButton.QinJi);
    }

    void HandleButtonClick(onButton button)
    {
        playerSequence.Add(button);
        Debug.Log("当前输入序列: " + string.Join(", ", playerSequence));
        ValidateSequence();
    }

    void ValidateSequence()
    {
        if (timeLog == null)
        {
            Debug.LogError("TimeLog 未绑定！请在 Inspector 中赋值。");
            return;
        }

        // 判断时间是否在 18:45 ~ 19:15 之间
        float totalMinutes = timeLog.hours * 60 + timeLog.minutes;
        bool isTimeInRange = totalMinutes >= 1125 && totalMinutes <= 1155; // 18:45=1125, 19:15=1155

        if (isTimeInRange)
        {
            tiShiLuo.SetActive(true);

            if (playerSequence.Count == LuoGen.Length)
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
                    Debug.Log("验证成功！");
                    winNum++;
                    Win.SetActive(true);
                    isResetting = true;
                }
                else
                {
                    Debug.Log("验证失败！");
                    Lose.SetActive(true);
                    isResetting = true;
                }
            }
        }
        else
        {
            tiShiLuo.SetActive(false);
        }
    }

    void ResetGame()
    {
        playerSequence.Clear();
        Win.SetActive(false);
        Lose.SetActive(false);
    }
}

    

    //void Qiao()
    //{
    //    if (timeLog.hours == 19 || timeLog.hours == 18)
    //    {
    //        if (timeLog.minutes >= 45 || timeLog.minutes <= 15)
    //        {
    //            if (Zhong == true)
    //            {
    //                Zhong = false;
                    
                    
    //            }
                
    //        }
    //    }
    //}

