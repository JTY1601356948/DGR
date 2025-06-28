using System.ComponentModel;
using UnityEngine;
using static EventController;

public class Fire : MonoBehaviour
{
    int fire = 0;
    int water = 0;
    public static int money = 0;
    bool OntriggerWater = false;

    public GameObject TiShiTianGui;
    public GameObject TiShiFire;
    public GameObject TiShiFireKill;
    private float resetTimer = 0f;
    private bool isResetting = false;
    private float LeaveFireTime = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TiShiTianGui.SetActive(false);
        TiShiFire.SetActive(false);
        TiShiFireKill.SetActive(false);
        if(collision.CompareTag("Water"))
        {
            OntriggerWater = true;
        }
        if(collision.CompareTag("FireCollision"))
        {
            TiShiFire.SetActive(true);
            isResetting = true;
        }
        if (collision.CompareTag("Fire"))
        {
            
            if (water > 0)
            {
                Destroy(collision.gameObject);
                water -= 1;
                money += 10;
                TiShiFireKill.SetActive(true);
            }
            else if(water ==0)
            {
                Debug.Log("请寻找水源");
            }

        }

        if (collision.CompareTag("TianGui"))
        {
            TiShiTianGui.SetActive(true);
            isResetting = true;
        }
    }

    void HandleResetTimer()
    {
        if (isResetting)
        {
            resetTimer += Time.deltaTime;
            //LeaveFireTime += Time.deltaTime;
            if (resetTimer >= 3f)
            {
                ResetGame();
                isResetting = false;
                resetTimer = 0f;
            }
            if(resetTimer>=20f)
            {
                money -= 100;
            }
        }
    }


    


    void ResetGame()
    {
        TiShiTianGui.SetActive(false);
        TiShiFire.SetActive(false) ;
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
            Debug.Log("接水");
        }


    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(20, 20, 500, 500), "剩余水源:" + water+"桶");
        GUI.Label(new Rect(20, 50, 500, 500), "铜钱:" + money + "串");


    }
}