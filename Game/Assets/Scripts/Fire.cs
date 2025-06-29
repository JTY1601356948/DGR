using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventController;

public class Fire : MonoBehaviour
{
    int fire = 0;
    int water = 0;
    int money = 0;
    int chance = 3;
    int firenumber = 0;
    bool OntriggerWater = false;
    private bool isGameOver = false; // ������Ϸ״̬��־
    public GameObject GameOverPanel;

    private float resetTimer = 0f;
    private bool isResetting = false;
    private float LeaveFireTime = 0f;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Water"))
        {
            OntriggerWater = true;
        }
        if(collision.CompareTag("FireCollision"))
        {
            firenumber += 1;
            isResetting = true;
        }
        if (collision.CompareTag("Fire"))
        {
            
            if (water > 0)
            {
                Destroy(collision.gameObject);
                water -= 1;
                firenumber-=1;
            }
            else if(water ==0)
            {
                Debug.Log("��Ѱ��ˮԴ");
            }

        }


    }

    void HandleResetTimer()
    {
        if (isResetting)
        {

            LeaveFireTime += Time.deltaTime;
            if (LeaveFireTime >= 20f && firenumber > 0)
            {
                chance -= 100;
            }
        }
    }

    void chanceCheck()
    {
            if (chance <= 0 && !isGameOver)
            {
                isGameOver = true;
                GameOver();
            }

    }
    void GameOver()
    {
        // ��ͣ��Ϸ����ѡ��
        Time.timeScale = 0f;
        Debug.Log("��Ϸʧ�ܣ�");
        ShowGameOverUI();
    }
    void ShowGameOverUI()
    {
        GameOverPanel.SetActive(true);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        OntriggerWater=false;
    }

    void Update()
    {
        if (OntriggerWater && Input.GetKeyDown(KeyCode.E))
        {
            water++;
            if (water > 1)
                water = 1;
            Debug.Log("��ˮ");
        }
        chanceCheck();

        HandleResetTimer();
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(20, 20, 500, 500), "ʣ��ˮԴ:" + water+"Ͱ");
        GUI.Label(new Rect(20, 50, 500, 500), "resettime+chance"+LeaveFireTime+"+"+chance);

    }
}