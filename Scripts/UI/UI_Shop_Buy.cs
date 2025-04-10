using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Shop_Buy : MonoBehaviour
{
    [SerializeField] bool isBuy = true;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject firstSelected;
    [SerializeField] TextMeshProUGUI moneyText;

    [Header("---- Sell Item ----")]
    [SerializeField] List<ShopSellData> sellItemData;
    [SerializeField] List<ShopSellData> sellWeaponData;
    [SerializeField] List<ShopSellData> sellGemData;
    [SerializeField] List<ShopSellData> sellAccessoriesData;

    [Header("---- Package Plane ----")]
    [SerializeField] UI_Shop_Buy_Package_Plane packagePlane;
    [SerializeField] UI_Shop_Buy_MessageBox buyMessageBox;
    [SerializeField] GameObject notEnoughMoneyMessageBox;
    [SerializeField] GameObject cannotBeSoldMessageBox;
    [SerializeField] IntEventChannel clickPackageItemEventChannel;

    int clickPackageItemIndex;

    private void OnEnable()
    {
        clickPackageItemEventChannel.AddListener(OpenBuyMessageBox);
        playerInput.onUnPause += OnCancel;
        playerInput.onCancel += OnCancel;

        RefreshMoneyText();
        ChangeBuyMessageBoxEnable(false);
        notEnoughMoneyMessageBox.SetActive(false);
        if (!isBuy) cannotBeSoldMessageBox.SetActive(false);
        if (isBuy) packagePlane.InitializeCommodity(sellItemData, sellWeaponData, sellGemData, sellAccessoriesData);
        packagePlane.ChangePackage(0);
        SetSelectedGameObject();
    }

    private void OnDisable()
    {
        clickPackageItemEventChannel.RemoveListener(OpenBuyMessageBox);
        playerInput.onUnPause -= OnCancel;
        playerInput.onCancel -= OnCancel;
    }

    private void OnCancel()
    {
        if (buyMessageBox.gameObject.activeSelf)
        {
            ChangeBuyMessageBoxEnable(false);
        }
        else if (notEnoughMoneyMessageBox.activeSelf)
        {
            notEnoughMoneyMessageBox.SetActive(false);
        }
        else if (!isBuy && cannotBeSoldMessageBox.activeSelf)
        {
            cannotBeSoldMessageBox.SetActive(false);
        }
        else
        {
            GameManager.Instance.UnPause();
            gameObject.SetActive(false);
        }
    }

    public void SetSelectedGameObject()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void ChangeBuyMessageBoxEnable(bool value)
    {
        buyMessageBox.gameObject.SetActive(value);
        packagePlane.ChangeEnableBuyMessageBox(value);
    }

    private void RefreshMoneyText()
    {
        moneyText.text = GameManager.Instance.money.ToString();
    }

    private void OpenBuyMessageBox(int index)
    {
        if (index == -1)
        {
            if (!isBuy) cannotBeSoldMessageBox.SetActive(true);
            return;
        }

        clickPackageItemIndex = index;

        int price = 0;
        int maxCount = 1;
        if (isBuy)
        {
            switch (packagePlane.currenPackage)
            {
                case PackageType.Item:
                    price = Package_Item.Instance.GetItemDataByID(sellItemData[index].id).buyPrice;
                    maxCount = sellItemData[index].count;
                    break;
                case PackageType.Weapon:
                    price = Package_Weapon.Instance.GetWeaponStaticDataByID(sellWeaponData[index].id, sellWeaponData[index].level).buyPrice;
                    maxCount = sellWeaponData[index].count;
                    break;
                case PackageType.Gem:
                    price = Package_Gem.Instance.GetGemStaticDataByID(sellGemData[index].id, sellGemData[index].level).buyPrice;
                    maxCount = sellGemData[index].count;
                    break;
                case PackageType.Accessories:
                    price = Package_Accessories.Instance.GetAccessoriesStaticDataByID(sellAccessoriesData[index].id, sellAccessoriesData[index].level).buyPrice;
                    maxCount = sellAccessoriesData[index].count;
                    break;
            }
        }
        else
        {
            switch (packagePlane.currenPackage)
            {
                case PackageType.Item:
                    var item = Package_Item.Instance.LoadPackage()[index];
                    price = Package_Item.Instance.GetItemDataByID(item.id).sellPrice;
                    maxCount = item.count;
                    break;
                case PackageType.Weapon:
                    var weapon = Package_Weapon.Instance.LoadPackage()[index];
                    price = Package_Weapon.Instance.GetWeaponStaticDataByID(weapon.id, weapon.level).sellPrice;
                    maxCount = 1;
                    break;
                case PackageType.Gem:
                    var gem = Package_Gem.Instance.LoadPackage()[index];
                    price = Package_Gem.Instance.GetGemStaticDataByID(gem.id, gem.rarity).sellPrice;
                    maxCount = 1;
                    break;
                case PackageType.Accessories:
                    var accessories = Package_Accessories.Instance.LoadPackage()[index];
                    price = Package_Accessories.Instance.GetAccessoriesStaticDataByID(accessories.id, accessories.rarity).sellPrice;
                    maxCount = 1;
                    break;
            }
        }
        if (maxCount == -1) return;
        if (maxCount == 0) maxCount = 99;

        ChangeBuyMessageBoxEnable(true);
        buyMessageBox.Initialize(price, maxCount);
    }

    public void BuyItem(int buyCount, int buyAllPrice)
    {
        if (isBuy)
        {
            if (GameManager.Instance.money > buyAllPrice)
            {
                GameManager.Instance.money -= buyAllPrice;

                switch (packagePlane.currenPackage)
                {
                    case PackageType.Item:
                        var item = sellItemData[clickPackageItemIndex];
                        Package_Item.Instance.CreatItem(item.id, item.level, buyCount);
                        if (item.count != 0)
                        {
                            item.count -= buyCount;
                            if (item.count == 0) sellItemData[clickPackageItemIndex].count = -1;
                        }
                        break;
                    case PackageType.Weapon:
                        var weapon = sellWeaponData[clickPackageItemIndex];
                        Package_Weapon.Instance.CreatWeapon(weapon.id, weapon.level);
                        if (weapon.count != 0)
                        {
                            weapon.count -= buyCount;
                            if (weapon.count == 0) sellWeaponData[clickPackageItemIndex].count = -1;
                        }
                        break;
                    case PackageType.Gem:
                        var gem = sellGemData[clickPackageItemIndex];
                        Package_Gem.Instance.CreatGem(gem.id, gem.level);
                        if (gem.count != 0)
                        {
                            gem.count -= buyCount;
                            if (gem.count == 0) sellGemData[clickPackageItemIndex].count = -1;
                        }
                        break;
                    case PackageType.Accessories:
                        var accessories = sellAccessoriesData[clickPackageItemIndex];
                        Package_Accessories.Instance.CreatAccessories(accessories.id, accessories.level);
                        if (accessories.count != 0)
                        {
                            accessories.count -= buyCount;
                            if (accessories.count == 0) sellAccessoriesData[clickPackageItemIndex].count = -1;
                        }
                        break;
                }
                SetSelectedGameObject();
            }
            else
            {
                notEnoughMoneyMessageBox.SetActive(true);
            }
        }
        else
        {
            GameManager.Instance.money = Mathf.Clamp(GameManager.Instance.money + buyAllPrice, 0, 1919114514);

            switch (packagePlane.currenPackage)
            {
                case PackageType.Item:
                    var item = Package_Item.Instance.LoadPackage()[clickPackageItemIndex];
                    Package_Item.Instance.RemoveItem(item.id, buyCount);
                    break;
                case PackageType.Weapon:
                    if (buyCount == 0) break;
                    var weapon = Package_Weapon.Instance.LoadPackage()[clickPackageItemIndex];
                    Package_Weapon.Instance.RemoveWeapon(weapon.uid);
                    break;
                case PackageType.Gem:
                    if (buyCount == 0) break;
                    var gem = Package_Gem.Instance.LoadPackage()[clickPackageItemIndex];
                    Package_Gem.Instance.RemoveGem(gem.uid);
                    break;
                case PackageType.Accessories:
                    if (buyCount == 0) break;
                    var accessories = Package_Accessories.Instance.LoadPackage()[clickPackageItemIndex];
                    Package_Accessories.Instance.RemoveAccessories(accessories.uid);
                    break;
            }
            SetSelectedGameObject();
        }
        ChangeBuyMessageBoxEnable(false);
        RefreshMoneyText();
        packagePlane.RefreshAll();
    }
}

[System.Serializable]
public class ShopSellData
{
    public string id;
    public int level;
    public int count;
}