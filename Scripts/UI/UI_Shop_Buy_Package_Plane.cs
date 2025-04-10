using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop_Buy_Package_Plane : UI_Package_Plane
{
    [SerializeField] bool isBuy = true;

    List<ItemDynamicData> itemDynamicDatas;
    List<WeaponDynamicData> weaponDynamicDatas;
    List<GemDynamicData> gemDynamicDatas;
    List<AccessoriesDynamicData> accessoriesDynamicDatas;
    bool isEnableBuyMessageBox;


    public void InitializeCommodity(List<ShopSellData> sellItemData, List<ShopSellData> sellWeaponData, List<ShopSellData> sellGemData, List<ShopSellData> sellAccessoriesData)
    {
        itemDynamicDatas = new();
        weaponDynamicDatas = new();
        gemDynamicDatas = new();
        accessoriesDynamicDatas = new();

        foreach (var sellItem in sellItemData)
        {
            itemDynamicDatas.Add(Package_Item.Instance.NewItem(sellItem.id, sellItem.level, 1));
        }
        foreach (var sellWeapon in sellWeaponData)
        {
            weaponDynamicDatas.Add(Package_Weapon.Instance.NewWeapon(sellWeapon.id, sellWeapon.level));
        }
        foreach (var sellGem in sellGemData)
        {
            gemDynamicDatas.Add(Package_Gem.Instance.NewGem(sellGem.id, sellGem.level));
        }
        foreach (var sellAccessories in sellAccessoriesData)
        {
            accessoriesDynamicDatas.Add(Package_Accessories.Instance.NewAccessories(sellAccessories.id, sellAccessories.level));
        }
    }

    public void ChangeEnableBuyMessageBox(bool value)
    {
        isEnableBuyMessageBox = value;
    }

    protected override void PackageIndexAddOne()
    {
        if (isEnableBuyMessageBox) return;

        base.PackageIndexAddOne();
    }

    protected override void PackageIndexRemoveOne()
    {
        if (isEnableBuyMessageBox) return;

        base.PackageIndexRemoveOne();
    }

    public override void RefreshAll()
    {
        if (isBuy)
        {
            RefreshContent();
            RefreshScroll(itemDynamicDatas,weaponDynamicDatas,gemDynamicDatas,accessoriesDynamicDatas, true);
        }
        else
        {
            RefreshContent();
            RefreshScroll(false, true);
        }
    }

    public override void RefreshInfo(int value, List<ItemDynamicData> itemDynamicDatas, List<WeaponDynamicData> weaponDynamicDatas, List<GemDynamicData> gemDynamicDatas, List<AccessoriesDynamicData> accessoriesDynamicDatas)
    {
        if (isBuy)
        {
            base.RefreshInfo(value, this.itemDynamicDatas, this.weaponDynamicDatas, this.gemDynamicDatas, this.accessoriesDynamicDatas);
        }
        else
        {
            base.RefreshInfo(value, itemDynamicDatas, weaponDynamicDatas, gemDynamicDatas, accessoriesDynamicDatas);
        }
    }
}
