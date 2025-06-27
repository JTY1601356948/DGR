using System.ComponentModel;
using UnityEngine;
using static EventController;

public class Fire : MonoBehaviour
{
    int fire = 0;
    int water = 0;
    int money = 0;
    bool OntriggerWater = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            OntriggerWater = true;
        }
        if (collision.CompareTag("Fire"))
        {
            
            if (water > 0)
            {
                Destroy(collision.gameObject);
                water -= 1;
                money += 10;
                Debug.Log("√ª");
            }
            else if(water ==0)
            {
                Debug.Log("«Î—∞’“ÀÆ‘¥");
            }

        }
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
            Debug.Log("Ω”ÀÆ");
        }


    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(20, 20, 500, 500), " £”‡ÀÆ‘¥:" + water+"Õ∞");
        GUI.Label(new Rect(20, 50, 500, 500), "Õ≠«Æ:" + money + "¥Æ");


    }
}