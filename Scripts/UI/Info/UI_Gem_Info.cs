using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gem_Info : MonoBehaviour
{
    [SerializeField] GameObject dynamic;
    [SerializeField] Image itemRarity;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] TextMeshProUGUI itemHealth;
    [SerializeField] TextMeshProUGUI itemPhysicalDefense;
    [SerializeField] TextMeshProUGUI itemMagicDefense;
    [SerializeField] TextMeshProUGUI itemFireAttack;
    [SerializeField] TextMeshProUGUI itemWoodenAttack;
    [SerializeField] TextMeshProUGUI itemWaterAttack;
    [SerializeField] TextMeshProUGUI itemRockAttack;
    [SerializeField] TextMeshProUGUI itemElectricAttack;
    [SerializeField] TextMeshProUGUI itemDarkAttack;

    public void Refresh(GemData gemData, int gemRarity)
    {
        dynamic.SetActive(true);

        itemRarity.sprite = GameManager.Instance.rarity[gemRarity];
        itemIcon.sprite = gemData.ItemIcon;
        itemName.text = gemData.ItemName;
        itemType.text = "寶石/";
        itemDescription.text = gemData.ItemDescription;
        itemHealth.text = gemData.gemStaticDataList[gemRarity].health.ToString();
        itemPhysicalDefense.text = gemData.gemStaticDataList[gemRarity].physicalDefense.ToString();
        itemMagicDefense.text = gemData.gemStaticDataList[gemRarity].magicDefense.ToString();
        itemFireAttack.text = gemData.gemStaticDataList[gemRarity].fireDefense.ToString();
        itemWoodenAttack.text = gemData.gemStaticDataList[gemRarity].woodenDefense.ToString();
        itemWaterAttack.text = gemData.gemStaticDataList[gemRarity].waterDefense.ToString();
        itemRockAttack.text = gemData.gemStaticDataList[gemRarity].rockDefense.ToString();
        itemElectricAttack.text = gemData.gemStaticDataList[gemRarity].electricDefense.ToString();
        itemDarkAttack.text = gemData.gemStaticDataList[gemRarity].darkDefense.ToString();
    }
    
    public void Null()
    {
        dynamic.SetActive(false);
    }
}
