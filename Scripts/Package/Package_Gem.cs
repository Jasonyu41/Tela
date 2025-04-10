using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package_Gem
{
    private static Package_Gem _instance;
    public static Package_Gem Instance
    { 
        get
        {
            if (_instance == null)
            {
                _instance = new Package_Gem();
            }
            return _instance;
        }
    }

    [SerializeField] public List<GemDynamicData> package;


#region Package
    public List<GemDynamicData> LoadPackage()
    {
        if (package != null)
        {
            return package;
        }

        package = new List<GemDynamicData>();

        return package;
    }
#endregion


#region Gem
    public void CreatGem(string id, int rarity, bool isEquipment = false, int equipmentMember = 0, int equipmentIndex = 0)
    {
        GemDynamicData gem;

        gem = NewGem(id, rarity, isEquipment);
        AddGem(gem);

        if (isEquipment)
        {
            PartyManager.party[equipmentMember].EquipmentItem(PackageType.Gem, gem.uid, equipmentIndex);
        }
    }

    public GemDynamicData NewGem(string id, int rarity, bool isEquipment = false)
    {
        GemDynamicData gem = new();

        gem.uid = Guid.NewGuid().ToString();
        gem.id = id;
        gem.rarity = rarity;
        gem.isEquipment = isEquipment;

        return gem;
    }

    public void AddGem(GemDynamicData addGem)
    {
        LoadPackage().Add(addGem);
    }

    public void AddGem(List<GemDynamicData> addGemList)
    {
        foreach (GemDynamicData gem in addGemList)
        {
            AddGem(gem);
        }
    }

    public void RemoveGem(string uid)
    {
        GemDynamicData removeGem = GetGemDynamicDataByUID(uid);
        if (removeGem == null) return;

        package.Remove(removeGem);
    }

    public void RemoveGem(List<string> uidList)
    {
        foreach (string uid in uidList)
        {
            RemoveGem(uid);
        }
    }
#endregion


#region Get Gem Element
    public GemData GetGemDataByID(string id)
    {
        foreach (GemData gemData in GameManager.Instance.gemDatas)
        {
            if (gemData.ItemID == id)
            {
                return gemData;
            }
        }
        return null;
    }

    public GemStaticData GetGemStaticDataByID(string id, int rarity)
    {
        foreach (GemData gemData in GameManager.Instance.gemDatas)
        {
            if (gemData.ItemID == id)
            {
                return gemData.gemStaticDataList[rarity];
            }
        }
        return null;
    }

    public GemDynamicData GetGemDynamicDataByUID(string uid)
    {
        foreach (GemDynamicData gemDynamicData in LoadPackage())
        {
            if (gemDynamicData.uid == uid)
            {
                return gemDynamicData;
            }
        }
        return null;
    }

    public int GetGemInPackageIndex(GemDynamicData gem)
    {
        return LoadPackage().IndexOf(gem);
    }
#endregion


    public List<GemDynamicData> GetSortGemDynamicData()
    {
        LoadPackage().Sort(new GemDataComparer());
        return package;
    }
}

public class GemDataComparer : IComparer<GemDynamicData>
{
    public int Compare(GemDynamicData x, GemDynamicData y)
    {
        int equipmentComparison = y.isEquipment.CompareTo(x.isEquipment);
        if (equipmentComparison == 0)
        {
            int rarityComparison = y.rarity.CompareTo(x.rarity);
            if (rarityComparison == 0)
            {
                GemStaticData a = Package_Gem.Instance.GetGemStaticDataByID(x.id, x.rarity);
                GemStaticData b = Package_Gem.Instance.GetGemStaticDataByID(y.id, y.rarity);

                int healthComparison = b.health.CompareTo(a.health);
                if (healthComparison == 0)
                {
                    return y.id.CompareTo(x.id);
                }
                return healthComparison;
            }
            return rarityComparison;
        }
        return equipmentComparison;
    }
}