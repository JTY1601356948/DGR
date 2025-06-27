using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class transform : MonoBehaviour
{
    public GameObject targetTeleport;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = targetTeleport.transform.position;
        }
    }
}
