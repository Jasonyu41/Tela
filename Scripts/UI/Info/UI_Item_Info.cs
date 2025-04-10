using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item_Info : MonoBehaviour
{
    [SerializeField] GameObject dynamic;
    [SerializeField] Image itemRarity;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] TextMeshProUGUI itemEffects;

    public void Refresh(ItemData itemData, int rarity)
    {
        dynamic.SetActive(true);

        itemRarity.sprite = GameManager.Instance.rarity[rarity];
        itemIcon.sprite = itemData.ItemIcon;
        itemName.text = itemData.ItemName;
        itemType.text = "道具";
        itemDescription.text = itemData.ItemDescription;
        itemEffects.text = itemData.ItemEffects;
    }

    public void Null()
    {
        dynamic.SetActive(false);
    }
}
