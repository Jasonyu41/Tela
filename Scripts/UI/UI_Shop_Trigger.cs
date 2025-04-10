using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop_Trigger : MonoBehaviour
{
    [SerializeField] Canvas shopInfoCanvas;
    [HideInInspector] public bool isEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            shopInfoCanvas.enabled = true;
            isEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            shopInfoCanvas.enabled = false;
            isEnter = false;
        }
    }
}
