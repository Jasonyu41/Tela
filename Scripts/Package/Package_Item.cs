using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package_Item
{
    private static Package_Item _instance;
    public static Package_Item Instance
    { 
        get
        {
            if (_instance == null)
            {
                _instance = new Package_Item();
            }
            return _instance;
        }
    }

    [SerializeField] public List<ItemDynamicData> package;


#region Package
    public List<ItemDynamicData> LoadPackage()
    {
        if (package != null)
        {
            return package;
        }

        package = new List<ItemDynamicData>();

        return package;
    }
#endregion


#region Item
    public void CreatItem(string id, int rarity, int count, bool isEquipment = false, int equipmentMember = 0, int equipmentIndex = 0)
    {
        ItemDynamicData item;

        item = NewItem(id, rarity, count, isEquipment);
        AddItem(item);

        if (isEquipment)
        {
            PartyManager.party[equipmentMember].EquipmentItem(PackageType.Item, item.id, equipmentIndex);
        }
    }

    public ItemDynamicData NewItem(string id, int rarity, int count, bool isEquipment = false)
    {
        ItemDynamicData item = new();

        item.id = id;
        item.rarity = rarity;
        item.count = count;
        item.isEquipment = isEquipment;

        return item;
    }

    public void AddItem(string id, int count)
    {
        ItemDynamicData packageItem = GetItemDynamicDataByID(id);
        if (packageItem == null)
        {
            ItemDynamicData addItem = new ItemDynamicData();
            
            addItem.id = id;
            addItem.count = count;

            LoadPackage().Add(addItem);
        }
        else
        {
            packageItem.count += count;
        }
    }

    public void AddItem(List<string> idList, List<int> countList)
    {
        for (int i = 0; i < idList.Count; i++)
        {
            AddItem(idList[i], countList[i]);
        }
    }

    public void RemoveItem(string id, int count)
    {
        ItemDynamicData packageItem = GetItemDynamicDataByID(id);
        if (packageItem == null) return;

        packageItem.count -= count;

        if (packageItem.count <= 0) package.Remove(packageItem);
    }

    public void RemoveItem(List<string> idList, List<int> countList)
    {
        for (int i = 0; i < idList.Count; i++)
        {
            RemoveItem(idList[i], countList[i]);
        }
    }

    public void AddItem(ItemDynamicData addItem)
    {
        ItemDynamicData packageItem = GetItemDynamicDataByID(addItem.id);
        if (packageItem == null)
        {
            LoadPackage().Add(addItem);
        }
        else
        {
            packageItem.count += addItem.count;
        }
    }

    public void AddItem(List<ItemDynamicData> addItemList)
    {
        foreach (ItemDynamicData item in addItemList)
        {
            AddItem(item);
        }
    }

    public void RemoveItem(ItemDynamicData removeItem)
    {
        ItemDynamicData packageItem = GetItemDynamicDataByID(removeItem.id);
        if (packageItem == null) return;

        packageItem.count -= removeItem.count;
        
        if (packageItem.count <= 0) package.Remove(packageItem);
    }

    public void RemoveItem(List<ItemDynamicData> removeItemList)
    {
        foreach (ItemDynamicData item in removeItemList)
        {
            RemoveItem(item);
        }
    }
#endregion


#region Get Item Element
    public ItemData GetItemDataByID(string id)
    {
        foreach (ItemData itemData in GameManager.Instance.itemDatas)
        {
            if (itemData.ItemID == id)
            {
                return itemData;
            }
        }
        return null;
    }

    public ItemDynamicData GetItemDynamicDataByID(string id)
    {
        foreach (ItemDynamicData itemDynamicData in LoadPackage())
        {
            if (itemDynamicData.id == id)
            {
                return itemDynamicData;
            }
        }
        return null;
    }

    public int GetItemInPackageIndex(ItemDynamicData item)
    {
        return LoadPackage().IndexOf(item);
    }
#endregion


    public List<ItemDynamicData> GetSortItemDynamicData()
    {
        LoadPackage().Sort(new ItemDataComparer());
        return package;
    }
}

public class ItemDataComparer : IComparer<ItemDynamicData>
{
    public int Compare(ItemDynamicData x, ItemDynamicData y)
    {
        int equipmentComparison = y.isEquipment.CompareTo(x.isEquipment);
        if (equipmentComparison == 0)
        {
            int countComparison = y.count.CompareTo(x.count);
            if (countComparison == 0)
            {
                return y.id.CompareTo(x.id);
            }
            return countComparison;
        }
        return equipmentComparison;
    }
}