using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPPlayerLookAtBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.canLookAtBoss = true;
            gameObject.SetActive(false);
        }
    }
}
