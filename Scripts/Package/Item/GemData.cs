using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Gem", fileName = "Gem_")]
public class GemData : Item
{
    [Header("Gem Data")]
    [SerializeField] TextAsset gemRarityDataFile;
    public List<GemStaticData> gemStaticDataList;

    private void OnValidate()
    {
        if (!gemRarityDataFile) return;

        gemStaticDataList.Clear();
        
        string[] textInLines = gemRarityDataFile.text.Split('\n');

        for (int i = 1; i < textInLines.Length; i++)
        {
            if (string.IsNullOrEmpty(textInLines[i])) break;
            string[] statsValues = textInLines[i].Split(",");

            GemStaticData currentGemRarityData = new GemStaticData();

            currentGemRarityData.health = float.Parse(statsValues[1]);
            currentGemRarityData.physicalDefense = float.Parse(statsValues[2]);
            currentGemRarityData.magicDefense = float.Parse(statsValues[3]);
            currentGemRarityData.fireDefense = float.Parse(statsValues[4]);
            currentGemRarityData.woodenDefense = float.Parse(statsValues[5]);
            currentGemRarityData.waterDefense = float.Parse(statsValues[6]);
            currentGemRarityData.rockDefense = float.Parse(statsValues[7]);
            currentGemRarityData.electricDefense = float.Parse(statsValues[8]);
            currentGemRarityData.darkDefense = float.Parse(statsValues[9]);
            currentGemRarityData.buyPrice = int.Parse(statsValues[10]);
            currentGemRarityData.sellPrice = int.Parse(statsValues[11]);

            gemStaticDataList.Add(currentGemRarityData);
        }
    }
}

[System.Serializable]
public class GemStaticData
{
    public float health;
    public float physicalDefense;
    public float magicDefense;
    public float fireDefense;
    public float woodenDefense;
    public float waterDefense;
    public float rockDefense;
    public float electricDefense;
    public float darkDefense;
    public int buyPrice;
    public int sellPrice;
}

[System.Serializable]
public class GemDynamicData
{
    public string uid;
    public string id;
    public int rarity;
    [HideInInspector] public bool isEquipment;
}
