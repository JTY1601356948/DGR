using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dialog : MonoBehaviour
{
    public GameObject dialog;
    public TMP_Text dialogText;
    public string signText;
    public bool isPlayerInSign;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerInSign)
        {
            dialog.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            dialogText.text = signText;
            isPlayerInSign = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isPlayerInSign = false;
            dialog.SetActive(false);
        }
    }
}
