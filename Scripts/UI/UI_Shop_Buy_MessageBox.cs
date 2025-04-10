using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Shop_Buy_MessageBox : MonoBehaviour
{
    [SerializeField] UI_Shop_Buy uI_Shop_Buy;
    [SerializeField] GameObject firstSelected;
    [SerializeField] TextMeshProUGUI buyCountText;
    [SerializeField] TextMeshProUGUI buyAllPriceText;

    int unitPrice;
    int maxCount;
    int buyCount;
    int buyAllPrice;


    public void Initialize(int unitPrice, int maxCount)
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        this.unitPrice = unitPrice;
        this.maxCount = maxCount;
        buyCount = 1;
        Refresh();
    }

    public void ChangeBuyCount(int value)
    {
        buyCount += value;
        buyCount = Mathf.Clamp(buyCount, 0, maxCount);
        
        Refresh();
    }

    private void Refresh()
    {
        buyAllPrice = unitPrice * buyCount;

        buyCountText.text = buyCount.ToString();
        buyAllPriceText.text = buyAllPrice.ToString();
    }

    public void BuyItem()
    {
        uI_Shop_Buy.BuyItem(buyCount, buyAllPrice);
    }

    public void Exit()
    {
        uI_Shop_Buy.SetSelectedGameObject();
        uI_Shop_Buy.ChangeBuyMessageBoxEnable(false);
    }
}
