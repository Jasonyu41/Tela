using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Package_Plane : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [Header("---- Content Text ----")]
    [SerializeField] List<TextMeshProUGUI> contentButtonText;
    [SerializeField] TMP_FontAsset normalFont;
    [SerializeField] float normalFontSize;
    [SerializeField] Color normalFontColor;
    [SerializeField] TMP_FontAsset selectFont;
    [SerializeField] float selectFontSize;
    [SerializeField] Color selectFontColor;
    [SerializeField] Image contentSlider;
    [SerializeField] List<float> contentSliderPositionX;
    [SerializeField] float contentSliderMoveTime;

    [Header("---- Package Plane ----")]
    [SerializeField] Transform scrollContent;
    [SerializeField] UI_Item_Info itemInfo;
    [SerializeField] UI_Weapon_Info weaponInfo;
    [SerializeField] UI_Gem_Info gemInfo;
    [SerializeField] UI_Accessories_Info accessoriesInfo;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Sprite whiteItemIcon;
    [SerializeField] Sprite blackItemIcon;
    [SerializeField] int packageHorizontalCount = 5;
    [SerializeField] int minPackageItemCount = 25;

    public PackageType currenPackage { get; private set; }
    int currentPackageIndex;
    float contentSliderMoveTimer;
    Coroutine sliderMoveCoroutine;


    private void OnEnable()
    {
        playerInput.onUIMoveLeft += PackageIndexRemoveOne;
        playerInput.onUIMoveRight += PackageIndexAddOne;
    }

    private void OnDisable()
    {
        playerInput.onUIMoveLeft -= PackageIndexRemoveOne;
        playerInput.onUIMoveRight -= PackageIndexAddOne;
    }

    public void ChangePackage(int value)
    {
        switch (value)
        {
            case 0:
                currenPackage = PackageType.Item;
                break;
            case 1:
                currenPackage = PackageType.Weapon;
                break;
            case 2:
                currenPackage = PackageType.Gem;
                break;
            case 3:
                currenPackage = PackageType.Accessories;
                break;
        }

        currentPackageIndex = value;

        RefreshAll();
    }

    public virtual void RefreshAll()
    {
        RefreshContent();
        RefreshScroll();
    }

    protected virtual void PackageIndexAddOne()
    {
        currentPackageIndex = Mathf.Clamp(currentPackageIndex + 1, 0, 3);
        ChangePackage(currentPackageIndex);
    }

    protected virtual void PackageIndexRemoveOne()
    {
        currentPackageIndex = Mathf.Clamp(currentPackageIndex - 1, 0, 3);
        ChangePackage(currentPackageIndex);
    }

    public void RefreshContent()
    {
        for (int i = 0; i < contentButtonText.Count; i++)
        {
            if (i == currentPackageIndex)
            {
                contentButtonText[i].font = selectFont;
                contentButtonText[i].fontSize = selectFontSize;
                contentButtonText[i].color = selectFontColor;
            }
            else
            {
                contentButtonText[i].font = normalFont;
                contentButtonText[i].fontSize = normalFontSize;
                contentButtonText[i].color = normalFontColor;
            }
        }

        if (sliderMoveCoroutine != null) StopCoroutine(sliderMoveCoroutine);
        sliderMoveCoroutine = StartCoroutine(SliderMoveCortutine());
    }

    IEnumerator SliderMoveCortutine()
    {
        var originalPosition = contentSlider.transform.localPosition;
        var targetPosition = originalPosition;
        targetPosition.x = contentSliderPositionX[currentPackageIndex];

        contentSliderMoveTimer = 0;
        while (contentSliderMoveTimer < 1)
        {
            contentSliderMoveTimer += Time.unscaledDeltaTime / contentSliderMoveTime;
            contentSlider.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, contentSliderMoveTimer);

            yield return null;
        }
    }

    public void RefreshScroll(bool isShowBuyPrice = false, bool isShowSellPrice = false)
    {
        RefreshScroll(Package_Item.Instance.GetSortItemDynamicData()
                    , Package_Weapon.Instance.GetSortWeaponDynamicData()
                    , Package_Gem.Instance.GetSortGemDynamicData()
                    , Package_Accessories.Instance.GetSortAccessoriesDynamicData()
                    , isShowBuyPrice, isShowSellPrice);
    }

    public void RefreshScroll(List<ItemDynamicData> itemDynamicDatas
                            , List<WeaponDynamicData> weaponDynamicDatas
                            , List<GemDynamicData> gemDynamicDatas
                            , List<AccessoriesDynamicData> accessoriesDynamicDatas
                            , bool isShowBuyPrice = false, bool isShowSellPrice = false)
    {
        switch(currenPackage)
        {
            case PackageType.Item:
                RefreshContentItems(itemDynamicDatas.Count);
                for (int i = 0; i < itemDynamicDatas.Count; i++)
                {
                    var item = Package_Item.Instance.GetItemDataByID(itemDynamicDatas[i].id);
                    var packageItem = scrollContent.GetChild(i).GetComponent<UI_Package_Item>();
                    packageItem.Refresh(itemDynamicDatas[i].rarity, item.ItemIcon, this, i, itemDynamicDatas[i].count);
                    if (isShowBuyPrice) packageItem.RefreshMoney(item.buyPrice);
                    if (isShowSellPrice) packageItem.RefreshMoney(item.sellPrice);
                    if (itemDynamicDatas[i].isEquipment)
                    {
                        foreach (var player in PartyManager.party)
                        {
                            if (player.items.Exists(x => x == itemDynamicDatas[i].id))
                            {
                                packageItem.RefreshEquipmentSprite(player.playerEquipment);
                                break;
                            }
                        }
                    }
                }
                RefreshInfo(0, itemDynamicDatas, weaponDynamicDatas, gemDynamicDatas, accessoriesDynamicDatas);
                break;
            case PackageType.Weapon:
                RefreshContentItems(weaponDynamicDatas.Count);
                for (int i = 0; i < weaponDynamicDatas.Count; i++)
                {
                    var weapon = Package_Weapon.Instance.GetWeaponDataByID(weaponDynamicDatas[i].id);
                    var packageItem = scrollContent.GetChild(i).GetComponent<UI_Package_Item>();
                    packageItem.Refresh(0, weapon.ItemIcon, this, i);
                    if (isShowBuyPrice) packageItem.RefreshMoney(weapon.weaponStaticDataList[weaponDynamicDatas[i].level - 1].buyPrice);
                    if (isShowSellPrice) packageItem.RefreshMoney(weapon.weaponStaticDataList[weaponDynamicDatas[i].level - 1].sellPrice);
                    if (weaponDynamicDatas[i].isEquipment)
                    {
                        foreach (var player in PartyManager.party)
                        {
                            if (player.weapons.Exists(x => x == weaponDynamicDatas[i].uid))
                            {
                                packageItem.RefreshEquipmentSprite(player.playerEquipment);
                                break;
                            }
                        }
                    }
                }
                RefreshInfo(0, itemDynamicDatas, weaponDynamicDatas, gemDynamicDatas, accessoriesDynamicDatas);
                break;
            case PackageType.Gem:
                RefreshContentItems(gemDynamicDatas.Count);
                for (int i = 0; i < gemDynamicDatas.Count; i++)
                {
                    var gem = Package_Gem.Instance.GetGemDataByID(gemDynamicDatas[i].id);
                    var packageItem =scrollContent.GetChild(i).GetComponent<UI_Package_Item>();
                    packageItem.Refresh(gemDynamicDatas[i].rarity, gem.ItemIcon, this, i);
                    if (isShowBuyPrice) packageItem.RefreshMoney(gem.gemStaticDataList[gemDynamicDatas[i].rarity].buyPrice);
                    if (isShowSellPrice) packageItem.RefreshMoney(gem.gemStaticDataList[gemDynamicDatas[i].rarity].sellPrice);
                    if (gemDynamicDatas[i].isEquipment)
                    {
                        foreach (var player in PartyManager.party)
                        {
                            if (player.gems.Exists(x => x == gemDynamicDatas[i].uid))
                            {
                                packageItem.RefreshEquipmentSprite(player.playerEquipment);
                                break;
                            }
                        }
                    }
                }
                RefreshInfo(0, itemDynamicDatas, weaponDynamicDatas, gemDynamicDatas, accessoriesDynamicDatas);
                break;
            case PackageType.Accessories:
                RefreshContentItems(accessoriesDynamicDatas.Count);
                for (int i = 0; i < accessoriesDynamicDatas.Count; i++)
                {
                    var accessories = Package_Accessories.Instance.GetAccessoriesDataByID(accessoriesDynamicDatas[i].id);
                    var packageItem =scrollContent.GetChild(i).GetComponent<UI_Package_Item>();
                    packageItem.Refresh(accessoriesDynamicDatas[i].rarity, accessories.ItemIcon, this, i);
                    if (isShowBuyPrice) packageItem.RefreshMoney(accessories.accessoriesStaticDataList[accessoriesDynamicDatas[i].rarity].buyPrice);
                    if (isShowSellPrice) packageItem.RefreshMoney(accessories.accessoriesStaticDataList[accessoriesDynamicDatas[i].rarity].sellPrice);
                    if (accessoriesDynamicDatas[i].isEquipment)
                    {
                        foreach (var player in PartyManager.party)
                        {
                            if (player.accessories.Exists(x => x == accessoriesDynamicDatas[i].uid))
                            {
                                packageItem.RefreshEquipmentSprite(player.playerEquipment);
                                break;
                            }
                        }
                    }
                }
                RefreshInfo(0, itemDynamicDatas, weaponDynamicDatas, gemDynamicDatas, accessoriesDynamicDatas);
                break;
        }
        
        RefreshAllItemMask();
    }

    private void RefreshContentItems(int itemsCount)
    {
        int makeUpItemsCount = itemsCount + packageHorizontalCount - (itemsCount % packageHorizontalCount);
        if (makeUpItemsCount < minPackageItemCount) makeUpItemsCount = minPackageItemCount;

        while (scrollContent.childCount < makeUpItemsCount)
        {
            Instantiate(itemPrefab, scrollContent);
        }

        for (int i = 0; i < scrollContent.childCount; i++)
        {
            if (i < makeUpItemsCount)
            {
                scrollContent.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                scrollContent.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = itemsCount; i < makeUpItemsCount; i++)
        {
            switch (i % 5)
            {
                case 0:
                case 2:
                case 4:
                    RefreshNullItemIcon(scrollContent, i);
                    break;
                case 1:
                case 3:
                    RefreshNullItemIcon(scrollContent, i, false);
                    break;
            }
        }
    }

    public void RefreshNullItemIcon(Transform targetParent, int index, bool isWhite = true)
    {
        targetParent.GetChild(index).GetComponent<UI_Package_Item>().Refresh((isWhite? whiteItemIcon : blackItemIcon), this);
    }

    public void RefreshAllItemMask()
    {
        foreach (var child in scrollContent.GetComponentsInChildren<UI_Package_Item>())
        {
            child.RefreshMask(false);
        }
    }

    public void RefreshAllItemMask(int ignoreIndex)
    {
        foreach (var child in scrollContent.GetComponentsInChildren<UI_Package_Item>())
        {
            child.RefreshMask(true);
        }
        scrollContent.GetChild(ignoreIndex).GetComponent<UI_Package_Item>().RefreshMask(false);
    }

    public void RefreshInfo(int value)
    {
        RefreshInfo(value, Package_Item.Instance.LoadPackage(), Package_Weapon.Instance.LoadPackage(), Package_Gem.Instance.LoadPackage(), Package_Accessories.Instance.LoadPackage());
    }

    public virtual void RefreshInfo(int value
                            , List<ItemDynamicData> itemDynamicDatas
                            , List<WeaponDynamicData> weaponDynamicDatas
                            , List<GemDynamicData> gemDynamicDatas
                            , List<AccessoriesDynamicData> accessoriesDynamicDatas)
    {
        switch (currenPackage)
        {
            case PackageType.Item:
                itemInfo.gameObject.SetActive(true);
                weaponInfo.gameObject.SetActive(false);
                gemInfo.gameObject.SetActive(false);
                accessoriesInfo.gameObject.SetActive(false);

                if (itemDynamicDatas.Count > 0)
                {
                    var item = itemDynamicDatas[value];
                    itemInfo.Refresh(Package_Item.Instance.GetItemDataByID(item.id), item.rarity);
                }
                else
                {
                    itemInfo.Null();
                }
                break;
            case PackageType.Weapon:
                itemInfo.gameObject.SetActive(false);
                weaponInfo.gameObject.SetActive(true);
                gemInfo.gameObject.SetActive(false);
                accessoriesInfo.gameObject.SetActive(false);

                if (weaponDynamicDatas.Count > 0)
                {
                    var weapon = weaponDynamicDatas[value];
                    weaponInfo.Refresh(Package_Weapon.Instance.GetWeaponDataByID(weapon.id), weapon.level - 1);
                }
                else
                {
                    weaponInfo.Null();
                }
                break;
            case PackageType.Gem:
                itemInfo.gameObject.SetActive(false);
                weaponInfo.gameObject.SetActive(false);
                gemInfo.gameObject.SetActive(true);
                accessoriesInfo.gameObject.SetActive(false);

                if (gemDynamicDatas.Count > 0)
                {
                    var gem = gemDynamicDatas[value];
                    gemInfo.Refresh(Package_Gem.Instance.GetGemDataByID(gem.id), gem.rarity);
                }
                else
                {
                    gemInfo.Null();
                }
                break;
            case PackageType.Accessories:
                itemInfo.gameObject.SetActive(false);
                weaponInfo.gameObject.SetActive(false);
                gemInfo.gameObject.SetActive(false);
                accessoriesInfo.gameObject.SetActive(true);
                
                if (accessoriesDynamicDatas.Count > 0)
                {
                    var accessories = accessoriesDynamicDatas[value];
                    accessoriesInfo.Refresh(Package_Accessories.Instance.GetAccessoriesDataByID(accessories.id), accessories.rarity);
                }
                else
                {
                    accessoriesInfo.Null();
                }
                break;
        }
    }
}

public enum PackageType
{
    Item,
    Weapon,
    Gem,
    Accessories
}