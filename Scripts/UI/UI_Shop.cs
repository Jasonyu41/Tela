using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] UI_Shop_Trigger uIShopTrigger;
    [SerializeField] GameObject shopInfoGameObject;
    [SerializeField] UI_Shop_Buy uIShopBuy;
    [SerializeField] UI_Shop_Buy uIShopSell;
    [SerializeField] GameObject firstSeleced;

    private void OnEnable()
    {
        playerInput.onUnPause += OnCancel;
        playerInput.onCancel += OnCancel;
        playerInput.onInteracive += OpenInfo;
        shopInfoGameObject.SetActive(false);
    }

    private void OnDisable()
    {
        playerInput.onUnPause -= OnCancel;
        playerInput.onCancel -= OnCancel;
        playerInput.onInteracive -= OpenInfo;
    }

    public void OnCancel()
    {
        if (shopInfoGameObject.activeSelf)
        {
            GameManager.Instance.UnPause();
            shopInfoGameObject.SetActive(false);
        }
    }

    private void OpenInfo()
    {
        if (uIShopTrigger.isEnter && shopInfoGameObject.activeSelf == false)
        {
            shopInfoGameObject.SetActive(true);
            GameManager.Instance.Pause(false);
            EventSystem.current.SetSelectedGameObject(firstSeleced);
        }
    }

    public void OpenShopBuy()
    {
        // GameManager.Instance.Pause(false);
        uIShopBuy.gameObject.SetActive(true);
        shopInfoGameObject.SetActive(false);
    }

    public void OpenShopSell()
    {
        // GameManager.Instance.Pause(false);
        uIShopSell.gameObject.SetActive(true);
        shopInfoGameObject.SetActive(false);
    }
}
