using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package_Accessories
{
    private static Package_Accessories _instance;
    public static Package_Accessories Instance
    { 
        get
        {
            if (_instance == null)
            {
                _instance = new Package_Accessories();
            }
            return _instance;
        }
    }

    [SerializeField] public List<AccessoriesDynamicData> package;


#region Package
    public List<AccessoriesDynamicData> LoadPackage()
    {
        if (package != null)
        {
            return package;
        }

        package = new List<AccessoriesDynamicData>();

        return package;
    }
#endregion


#region Accessories
    public void CreatAccessories(string id, int rarity, bool isEquipment = false, int equipmentMember = 0, int equipmentIndex = 0)
    {
        AccessoriesDynamicData accessories;

        accessories = NewAccessories(id, rarity, isEquipment);
        AddAccessories(accessories);

        if (isEquipment)
        {
            PartyManager.party[equipmentMember].EquipmentItem(PackageType.Accessories, accessories.uid, equipmentIndex);
        }
    }

    public AccessoriesDynamicData NewAccessories(string id, int rarity, bool isEquipment = false)
    {
        AccessoriesDynamicData accessories = new();

        accessories.uid = Guid.NewGuid().ToString();
        accessories.id = id;
        accessories.rarity = rarity;
        accessories.isEquipment = isEquipment;

        return accessories;
    }

    public void AddAccessories(AccessoriesDynamicData addAccessories)
    {
        LoadPackage().Add(addAccessories);
    }

    public void AddAccessories(List<AccessoriesDynamicData> addAccessoriesList)
    {
        foreach (AccessoriesDynamicData accessories in addAccessoriesList)
        {
            AddAccessories(accessories);
        }
    }

    public void RemoveAccessories(string uid)
    {
        AccessoriesDynamicData removeAccessories = GetAccessoriesDynamicDataByUID(uid);
        if (removeAccessories == null) return;

        package.Remove(removeAccessories);
    }

    public void RemoveAccessories(List<string> uidList)
    {
        foreach (string uid in uidList)
        {
            RemoveAccessories(uid);
        }
    }
#endregion


#region Get Accessories Element
    public AccessoriesData GetAccessoriesDataByID(string id)
    {
        foreach (AccessoriesData accessoriesData in GameManager.Instance.accessoriesDatas)
        {
            if (accessoriesData.ItemID == id)
            {
                return accessoriesData;
            }
        }
        return null;
    }

    public AccessoriesStaticData GetAccessoriesStaticDataByID(string id, int rarity)
    {
        foreach (AccessoriesData accessoriesData in GameManager.Instance.accessoriesDatas)
        {
            if (accessoriesData.ItemID == id)
            {
                return accessoriesData.accessoriesStaticDataList[rarity];
            }
        }
        return null;
    }

    public AccessoriesDynamicData GetAccessoriesDynamicDataByUID(string uid)
    {
        foreach (AccessoriesDynamicData accessoriesDynamicData in LoadPackage())
        {
            if (accessoriesDynamicData.uid == uid)
            {
                return accessoriesDynamicData;
            }
        }
        return null;
    }

    public int GetAccessoriesInPackageIndex(AccessoriesDynamicData accessories)
    {
        return LoadPackage().IndexOf(accessories);
    }
#endregion


    public List<AccessoriesDynamicData> GetSortAccessoriesDynamicData()
    {
        LoadPackage().Sort(new AccessoriesDataComparer());
        return package;
    }
}

public class AccessoriesDataComparer : IComparer<AccessoriesDynamicData>
{
    public int Compare(AccessoriesDynamicData x, AccessoriesDynamicData y)
    {
        int equipmentComparison = y.isEquipment.CompareTo(x.isEquipment);
        if (equipmentComparison == 0)
        {
            int rarityComparison = y.rarity.CompareTo(x.rarity);
            if (rarityComparison == 0)
            {
                return y.id.CompareTo(x.id);
            }
            return rarityComparison;
        }
        return equipmentComparison;
    }
}