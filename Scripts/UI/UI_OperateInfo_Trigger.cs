using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OperateInfo_Trigger : MonoBehaviour
{
    [SerializeField] UI_OperateInfo UI_OperateInfo;

    bool isFirst = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isFirst && other.tag == "Player")
        {
            UI_OperateInfo.gameObject.SetActive(true);
            isFirst = true;
        }
    }
}
