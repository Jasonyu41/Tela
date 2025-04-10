using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Accessories_Info : MonoBehaviour
{
    [SerializeField] GameObject dynamic;
    [SerializeField] Image itemRarity;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] TextMeshProUGUI itemEffects;
    [SerializeField] TextMeshProUGUI[] stats_Text;
    [SerializeField] TextMeshProUGUI[] stats;

    const string CRITICALHIT = "爆擊率";
    const string CRITICALHITHITDAMAGE = "爆擊傷害";
    const string MOVESPEED = "移動速度";
    const string ATTACKSPEED = "攻擊速度";
    const string COOLDOWN = "技能冷卻";
    const string LUCK = "幸運值";

    int index;

    public void Refresh(AccessoriesData accessoriesData, int accessoriesRarity)
    {
        dynamic.SetActive(true);

        itemRarity.sprite = GameManager.Instance.rarity[accessoriesRarity];
        itemIcon.sprite = accessoriesData.ItemIcon;
        itemName.text = accessoriesData.ItemName;
        itemType.text = "飾品";
        itemDescription.text = accessoriesData.ItemDescription;

        index = 0;

        if (accessoriesData.accessoriesEffects == string.Empty)
        {
            itemEffects.text = string.Empty;
            
            isZero(accessoriesData.accessoriesStaticDataList[accessoriesRarity].criticalHit, CRITICALHIT);
            isZero(accessoriesData.accessoriesStaticDataList[accessoriesRarity].criticalHitDamage, CRITICALHITHITDAMAGE);
            isZero(accessoriesData.accessoriesStaticDataList[accessoriesRarity].moveSpeed, MOVESPEED);
            isZero(accessoriesData.accessoriesStaticDataList[accessoriesRarity].attackSpeed, ATTACKSPEED);
            isZero(accessoriesData.accessoriesStaticDataList[accessoriesRarity].coolDown, COOLDOWN);
            isZero(accessoriesData.accessoriesStaticDataList[accessoriesRarity].luck, LUCK);
        }
        else
        {
            itemEffects.text = accessoriesData.accessoriesEffects;
        }

        for (int i = index; i < stats_Text.Length; i++)
        {
            stats_Text[i].text = string.Empty;
            stats[i].text = string.Empty;
        }
    }

    private void isZero(float value, string constString)
    {
        if (value > 0)
        {
            stats_Text[index].text = constString;
            stats[index].text = value.ToString();
            index ++;
        }
    }
    
    public void Null()
    {
        dynamic.SetActive(false);
    }
}
