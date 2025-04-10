using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Package_Item_Shop : UI_Package_Item
{
    [SerializeField] IntEventChannel clickPackageItemEventChannel;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (packageIndex == -1) return;

        clickPackageItemEventChannel.Broadcast(isEquipment? -1 : packageIndex);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if (packageIndex == -1) return;

        clickPackageItemEventChannel.Broadcast(isEquipment? -1 : packageIndex);
    }
}
