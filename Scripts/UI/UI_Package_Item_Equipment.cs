using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Package_Item_Equipment : UI_Package_Item
{
    [SerializeField] IntEventChannel clickEquipmentItemEventChannel;

    int equipmentIndex;

    public override void OnPointerClick(PointerEventData eventData)
    {
        RefreshItem();
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        RefreshItem();
    }

    private void RefreshItem()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).name == name)
            {
                equipmentIndex = i;
                break;
            }
        }

        clickEquipmentItemEventChannel.Broadcast(equipmentIndex);
    }
}
