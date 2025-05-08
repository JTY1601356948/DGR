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
public class ButtonClick : MonoBehaviour
{
    public Button ZhongJi;
    public Button QinJi;
    public TimeLog timeLog;
    private float resetTimer = 0f;
    private bool isResetting = false;

    public GameObject Win;
    public GameObject Lose;
    private List<onButton> playerSequence = new List<onButton>();

    public onButton[] LuoGen = new onButton[] { onButton.ZhongJi, onButton.QinJi, onButton.ZhongJi };

    void Start()
    {
        ZhongJi.onClick.AddListener(OnButtonZhong);
        QinJi.onClick.AddListener(OnButtonQin);
        Win.SetActive(false);
        Lose.SetActive(false);
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
        if(0<=Time.time&&Time.time<=30)
        {
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

