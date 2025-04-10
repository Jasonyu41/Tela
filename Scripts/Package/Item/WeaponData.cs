using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Weapon", fileName = "Weapon_")]
public class WeaponData : Item
{
    [Header("Weapon Data")]
    public DamageType damageType;
    [SerializeField] TextAsset weaponLevelDataFile;
    public List<WeaponStaticData> weaponStaticDataList;

    private void OnValidate()
    {
        if (!weaponLevelDataFile) return;

        weaponStaticDataList.Clear();
        
        string[] textInLines = weaponLevelDataFile.text.Split('\n');

        for (int i = 1; i < textInLines.Length; i++)
        {
            if (string.IsNullOrEmpty(textInLines[i])) break;
            string[] statsValues = textInLines[i].Split(",");

            WeaponStaticData currentWeaponLevelData = new WeaponStaticData();

            currentWeaponLevelData.health = float.Parse(statsValues[1]);
            currentWeaponLevelData.attack = float.Parse(statsValues[2]);
            currentWeaponLevelData.criticalHit = float.Parse(statsValues[3]);
            currentWeaponLevelData.fireAttack = float.Parse(statsValues[4]);
            currentWeaponLevelData.woodenAttack = float.Parse(statsValues[5]);
            currentWeaponLevelData.waterAttack = float.Parse(statsValues[6]);
            currentWeaponLevelData.rockAttack = float.Parse(statsValues[7]);
            currentWeaponLevelData.electricAttack = float.Parse(statsValues[8]);
            currentWeaponLevelData.darkAttack = float.Parse(statsValues[9]);
            currentWeaponLevelData.buyPrice = int.Parse(statsValues[10]);
            currentWeaponLevelData.sellPrice = int.Parse(statsValues[11]);

            weaponStaticDataList.Add(currentWeaponLevelData);
        }
    }
}

[System.Serializable]
public class WeaponStaticData
{
    public float health;
    public float attack;
    [Range(0f, 100f)] public float criticalHit;
    public float fireAttack;
    public float woodenAttack;
    public float waterAttack;
    public float rockAttack;
    public float electricAttack;
    public float darkAttack;
    public int buyPrice;
    public int sellPrice;
}

[System.Serializable]
public class WeaponDynamicData
{
    public string uid;
    public string id;
    public int level;
    [HideInInspector] public bool isEquipment;
}