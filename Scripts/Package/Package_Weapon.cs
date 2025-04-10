using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Package_Weapon
{
    private static Package_Weapon _instance;
    public static Package_Weapon Instance
    { 
        get
        {
            if (_instance == null)
            {
                _instance = new Package_Weapon();
            }
            return _instance;
        }
    }

    public List<WeaponDynamicData> package;

    // const string PLAYER_DATA_PACKAGE_WEAPONS_NAME = "PlayerDataPackageWeapons.sav";


#region Package
    // public void SavePackage()
    // {
    //     SaveManager.SaveByJson(PLAYER_DATA_PACKAGE_WEAPONS_NAME, this, true);
    // }

    public List<WeaponDynamicData> LoadPackage()
    {
        if (package != null)
        {
            return package;
        }

        // var loadData = SaveManager.LoadFromJson<Package_Weapon>(PLAYER_DATA_PACKAGE_WEAPONS_NAME);
        
        // if (loadData == null)
        // {
        //     package = new List<WeaponDynamicData>();
        //     SavePackage();
        // }
        // else
        // {
        //     package = loadData.package;
        // }
        package = new List<WeaponDynamicData>();
        return package;
    }

    // public void DeletePackage()
    // {
    //     SaveManager.DeleteSaveFile(PLAYER_DATA_PACKAGE_WEAPONS_NAME);
    // }
#endregion


#region Weapon
    public void CreatWeapon(string id, int level, bool isEquipment = false, int equipmentMember = 0, int equipmentIndex = 0)
    {
        WeaponDynamicData weapon;

        weapon = NewWeapon(id, level, isEquipment);
        AddWeapon(weapon);

        if (isEquipment)
        {
            PartyManager.party[equipmentMember].EquipmentItem(PackageType.Weapon, weapon.uid, equipmentIndex);
        }
    }

    public WeaponDynamicData NewWeapon(string id, int level, bool isEquipment = false)
    {
        WeaponDynamicData weapon = new();

        weapon.uid = Guid.NewGuid().ToString();
        weapon.id = id;
        weapon.level = level;
        weapon.isEquipment = isEquipment;

        return weapon;
    }

    public void AddWeapon(WeaponDynamicData addWeapon)
    {
        LoadPackage().Add(addWeapon);
    }

    public void AddWeapon(List<WeaponDynamicData> addWeaponList)
    {
        foreach (WeaponDynamicData weapon in addWeaponList)
        {
            AddWeapon(weapon);
        }
    }

    public void RemoveWeapon(string uid)
    {
        WeaponDynamicData removeWeapon = GetWeaponDynamicDataByUID(uid);
        if (removeWeapon == null) return;

        package.Remove(removeWeapon);
    }

    public void RemoveWeapon(List<string> uidList)
    {
        foreach (string uid in uidList)
        {
            RemoveWeapon(uid);
        }
    }
#endregion


#region Get Weapon Element
    public WeaponData GetWeaponDataByID(string id)
    {
        foreach (WeaponData weaponData in GameManager.Instance.weaponDatas)
        {
            if (weaponData.ItemID == id)
            {
                return weaponData;
            }
        }
        return null;
    }

    public WeaponStaticData GetWeaponStaticDataByID(string id, int level)
    {
        foreach (WeaponData weaponData in GameManager.Instance.weaponDatas)
        {
            if (weaponData.ItemID == id)
            {
                return weaponData.weaponStaticDataList[level - 1];
            }
        }
        return null;
    }

    public WeaponDynamicData GetWeaponDynamicDataByUID(string uid)
    {
        foreach (WeaponDynamicData weaponDynamicData in LoadPackage())
        {
            if (weaponDynamicData.uid == uid)
            {
                return weaponDynamicData;
            }
        }
        return null;
    }

    public int GetWeaponInPackageIndex(WeaponDynamicData weapon)
    {
        return LoadPackage().IndexOf(weapon);
    }
#endregion


    public List<WeaponDynamicData> GetSortWeaponDynamicData()
    {
        LoadPackage().Sort(new WeaponDataComparer());
        return package;
    }
}

public class WeaponDataComparer : IComparer<WeaponDynamicData>
{
    public int Compare(WeaponDynamicData x, WeaponDynamicData y)
    {
        int equipmentComparison = y.isEquipment.CompareTo(x.isEquipment);
        if (equipmentComparison == 0)
        {
            int levelComparison = y.level.CompareTo(x.level);
            if (levelComparison == 0)
            {
                WeaponStaticData a = Package_Weapon.Instance.GetWeaponStaticDataByID(x.id, x.level);
                WeaponStaticData b = Package_Weapon.Instance.GetWeaponStaticDataByID(y.id, y.level);

                int attackComparison = b.attack.CompareTo(a.attack);
                if (attackComparison == 0)
                {
                    int hpComparison = b.health.CompareTo(a.health);
                    if (hpComparison == 0)
                    {
                        return y.id.CompareTo(x.id);
                    }
                    return hpComparison;
                }
                return attackComparison;
            }
            return levelComparison;
        }
        return equipmentComparison;
    }
}