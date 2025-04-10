using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Accessories", fileName = "Accessories_")]
public class AccessoriesData : Item
{
    [Header("Accessories Data")]
    public string accessoriesEffects;
    [SerializeField] TextAsset accessoriesRarityDataFile;
    public List<AccessoriesStaticData> accessoriesStaticDataList;

    private void OnValidate()
    {
        if (!accessoriesRarityDataFile) return;

        accessoriesStaticDataList.Clear();
        
        string[] textInLines = accessoriesRarityDataFile.text.Split('\n');

        for (int i = 1; i < textInLines.Length; i++)
        {
            if (string.IsNullOrEmpty(textInLines[i])) break;
            string[] statsValues = textInLines[i].Split(",");

            AccessoriesStaticData currentAccessoriesRarityData = new AccessoriesStaticData();

            currentAccessoriesRarityData.criticalHit = float.Parse(statsValues[1]);
            currentAccessoriesRarityData.criticalHitDamage = float.Parse(statsValues[2]);
            currentAccessoriesRarityData.moveSpeed = float.Parse(statsValues[3]);
            currentAccessoriesRarityData.attackSpeed = float.Parse(statsValues[4]);
            currentAccessoriesRarityData.coolDown = float.Parse(statsValues[5]);
            currentAccessoriesRarityData.luck = float.Parse(statsValues[6]);
            currentAccessoriesRarityData.buyPrice = int.Parse(statsValues[7]);
            currentAccessoriesRarityData.sellPrice = int.Parse(statsValues[8]);

            accessoriesStaticDataList.Add(currentAccessoriesRarityData);
        }
    }
}

[System.Serializable]
public class AccessoriesStaticData
{
    public float criticalHit;
    public float criticalHitDamage;
    public float moveSpeed;
    public float attackSpeed;
    public float coolDown;
    public float luck;
    public int buyPrice;
    public int sellPrice;
}

[System.Serializable]
public class AccessoriesDynamicData
{
    public string uid;
    public string id;
    public int rarity;
    [HideInInspector] public bool isEquipment;
}
