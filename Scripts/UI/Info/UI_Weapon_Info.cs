using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Weapon_Info : MonoBehaviour
{
    [SerializeField] GameObject dynamic;
    [SerializeField] Image itemRarity;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] TextMeshProUGUI itemHealth;
    [SerializeField] TextMeshProUGUI itemAttack;
    [SerializeField] TextMeshProUGUI itemCriticalHit;
    [SerializeField] TextMeshProUGUI itemFireAttack;
    [SerializeField] TextMeshProUGUI itemWoodenAttack;
    [SerializeField] TextMeshProUGUI itemWaterAttack;
    [SerializeField] TextMeshProUGUI itemRockAttack;
    [SerializeField] TextMeshProUGUI itemElectricAttack;
    [SerializeField] TextMeshProUGUI itemDarkAttack;

    public void Refresh(WeaponData weaponData, int weaponLevel)
    {
        dynamic.SetActive(true);

        itemRarity.sprite = GameManager.Instance.rarity[0];
        itemIcon.sprite = weaponData.ItemIcon;
        itemName.text = weaponData.ItemName;
        itemType.text = "武器/" + (weaponData.damageType == DamageType.PhysicalDamage ? "物理" : "魔法");
        itemDescription.text = weaponData.ItemDescription;
        itemHealth.text = weaponData.weaponStaticDataList[weaponLevel].health.ToString();
        itemAttack.text = weaponData.weaponStaticDataList[weaponLevel].attack.ToString();
        itemCriticalHit.text = weaponData.weaponStaticDataList[weaponLevel].criticalHit.ToString();
        itemFireAttack.text = weaponData.weaponStaticDataList[weaponLevel].fireAttack.ToString();
        itemWoodenAttack.text = weaponData.weaponStaticDataList[weaponLevel].woodenAttack.ToString();
        itemWaterAttack.text = weaponData.weaponStaticDataList[weaponLevel].waterAttack.ToString();
        itemRockAttack.text = weaponData.weaponStaticDataList[weaponLevel].rockAttack.ToString();
        itemElectricAttack.text = weaponData.weaponStaticDataList[weaponLevel].electricAttack.ToString();
        itemDarkAttack.text = weaponData.weaponStaticDataList[weaponLevel].darkAttack.ToString();
    }
    
    public void Null()
    {
        dynamic.SetActive(false);
    }
}
