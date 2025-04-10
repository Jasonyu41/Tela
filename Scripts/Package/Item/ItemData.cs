using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Item", fileName = "Item_")]
public class ItemData : Item
{
    public string ItemEffects;
    public int buyPrice;
    public int sellPrice;
}

[System.Serializable]
public class ItemDynamicData
{
    public string id;
    public int rarity;
    public int count;
    [HideInInspector] public bool isEquipment;
}