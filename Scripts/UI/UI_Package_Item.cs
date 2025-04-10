using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Package_Item : Selectable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ICancelHandler
{
    [SerializeField] Image rarity;
    [SerializeField] Image icon;
    [SerializeField] Image equipment;
    [SerializeField] Image mask;
    [SerializeField] Image bold;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI moneyText;

    UI_Package_Plane packagePlane;
    protected int packageIndex;
    protected bool isEquipment;

    public void Refresh(Sprite icon, UI_Package_Plane packagePlane, int packageIndex = -1, int count = 1, bool isEquipment = false, Sprite equipmentSprite = null)
    {
        rarity.enabled = false;
        this.icon.sprite = icon;
        bold.enabled = false;
        this.packagePlane = packagePlane;
        this.packageIndex = packageIndex;
        moneyText.enabled = false;
        this.isEquipment = isEquipment;
        equipment.enabled = isEquipment;
        if (isEquipment) RefreshEquipmentSprite(equipmentSprite);
        countText.text = count > 1 ? count.ToString() : "";
    }

    public void Refresh(int rarity, Sprite icon, UI_Package_Plane packagePlane, int packageIndex = -1, int count = 1, bool isEquipment = false, Sprite equipmentSprite = null)
    {
        Refresh(icon, packagePlane, packageIndex, count, isEquipment, equipmentSprite);
        this.rarity.enabled = true;
        this.rarity.sprite = GameManager.Instance.rarity[rarity];
    }

    public void RefreshEquipmentSprite(Sprite equipmentSprite)
    {
        isEquipment = true;
        equipment.enabled = true;
        equipment.sprite = equipmentSprite;
    }

    public void RefreshMoney(int value)
    {
        moneyText.enabled = true;
        moneyText.text = value.ToString();
    }

    public void RefreshMask(bool active)
    {
        mask.enabled = active;
    }

#region Mouse
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        bold.enabled = true;
        if (packageIndex == -1) return;
        packagePlane.RefreshInfo(packageIndex);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        bold.enabled = false;
    }
#endregion

#region GamePad
    public virtual void OnSubmit(BaseEventData eventData)
    {

    }

    public void OnCancel(BaseEventData eventData)
    {

    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        bold.enabled = true;
        if (packageIndex == -1) return;
        packagePlane.RefreshInfo(packageIndex);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        bold.enabled = false;
    }
#endregion
}
