using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Package_Item_Package : UI_Package_Item
{
    [SerializeField] IntEventChannel clickPackageItemEventChannel;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isEquipment || packageIndex == -1) return;

        clickPackageItemEventChannel.Broadcast(packageIndex);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if (isEquipment || packageIndex == -1) return;

        clickPackageItemEventChannel.Broadcast(packageIndex);
    }
}
