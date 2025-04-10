using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Package : MonoBehaviour
{
    [SerializeField] Transform equipment;
    [SerializeField] List<Image> partyIcon;

    [Header("---- Package Plane ----")]
    [SerializeField] UI_Package_Plane packagePlane;
    [SerializeField] IntEventChannel clickEquipmentItemEventChannel;
    [SerializeField] IntEventChannel clickPackageItemEventChannel;
    [SerializeField] PlayerInput playerInput;

    PackageType currenPackage;
    int currentPackageIndex;
    Player currentPlayer;
    int equipmentIndex;
    int packageIndex;


    private void OnEnable()
    {
        clickEquipmentItemEventChannel.AddListener(ChangeEquipmentIndex);
        clickPackageItemEventChannel.AddListener(ChangePackageIndex);
        playerInput.onCancel += OnCancel;
        playerInput.onUIMoveLeft += PackageIndexRemoveOne;
        playerInput.onUIMoveRight += PackageIndexAddOne;

        RefreshAll();
    }

    private void OnDisable()
    {
        clickEquipmentItemEventChannel.RemoveListener(ChangeEquipmentIndex);
        clickPackageItemEventChannel.RemoveListener(ChangePackageIndex);
        playerInput.onCancel -= OnCancel;
        playerInput.onUIMoveLeft -= PackageIndexRemoveOne;
        playerInput.onUIMoveRight -= PackageIndexAddOne;
        
        gameObject.SetActive(false);
    }

    private void OnCancel()
    {
        GameManager.Instance.Pause();
        gameObject.SetActive(false);
    }

    private void RefreshAll()
    {
        currentPlayer = PartyManager.party[0];
        RefreshPartyIcon();
        ChangePackage(0);
        RefreshEquipment();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(partyIcon[0].gameObject);
    }

    public void ChangePlayer(int value)
    {
        currentPlayer =  PartyManager.party[value];
        for (int i = 0; i < partyIcon.Count; i++)
        {
            if (i != value) partyIcon[i].GetComponent<Animator>().SetTrigger("Normal");
        }

        RefreshPackage();
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

        equipmentIndex = -1;
        packageIndex = -1;
        RefreshEquipment();

        packagePlane.ChangePackage(value);
    }

    private void PackageIndexAddOne()
    {
        currentPackageIndex = Mathf.Clamp(currentPackageIndex + 1, 0, 3);
        ChangePackage(currentPackageIndex);
    }

    private void PackageIndexRemoveOne()
    {
        currentPackageIndex = Mathf.Clamp(currentPackageIndex - 1, 0, 3);
        ChangePackage(currentPackageIndex);
    }

    private void RefreshPartyIcon()
    {
        for (int i = 0; i < partyIcon.Count; i++)
        {
            partyIcon[i].sprite = PartyManager.party[i].playerBanner;
        }
    }

    public void RefreshPackage()
    {
        packagePlane.RefreshContent();
        packagePlane.RefreshScroll();
        RefreshEquipment();
    }

    public void RefreshEquipment()
    {
        switch (currenPackage)
        {
            case PackageType.Item:
                for (int i = 0; i < currentPlayer.items.Count; i++)
                {
                    if (string.IsNullOrEmpty(currentPlayer.items[i]))
                    {
                        RefreshNullEquipment(i);
                        continue;
                    }
                    var itemDynamicData = Package_Item.Instance.GetItemDynamicDataByID(currentPlayer.items[i]);
                    if (itemDynamicData != null)
                    {
                        equipment.GetChild(i).GetComponent<UI_Package_Item>().Refresh(itemDynamicData.rarity
                        , Package_Item.Instance.GetItemDataByID(itemDynamicData.id).ItemIcon
                        , packagePlane, Package_Item.Instance.GetItemInPackageIndex(itemDynamicData)
                        , itemDynamicData.count, true, currentPlayer.playerEquipment);
                    }
                    else
                    {
                        RefreshNullEquipment(i);
                    }
                }
                RefreshEquipmentChild(currentPlayer.equipmentItemCount);
                break;
            case PackageType.Weapon:
                for (int i = 0; i < currentPlayer.weapons.Count; i++)
                {
                    if (string.IsNullOrEmpty(currentPlayer.weapons[i]))
                    {
                        RefreshNullEquipment(i);
                        continue;
                    }
                    var weaponDynamicData = Package_Weapon.Instance.GetWeaponDynamicDataByUID(currentPlayer.weapons[i]);
                    if (weaponDynamicData != null)
                    {
                        equipment.GetChild(i).GetComponent<UI_Package_Item>().Refresh(0
                        , Package_Weapon.Instance.GetWeaponDataByID(weaponDynamicData.id).ItemIcon
                        , packagePlane, Package_Weapon.Instance.GetWeaponInPackageIndex(weaponDynamicData)
                        , 1, true, currentPlayer.playerEquipment);
                    }
                    else
                    {
                        RefreshNullEquipment(i);
                    }
                }
                RefreshEquipmentChild(currentPlayer.equipmentWeaponCount);
                break;
            case PackageType.Gem:
                for (int i = 0; i < currentPlayer.gems.Count; i++)
                {
                    if (string.IsNullOrEmpty(currentPlayer.gems[i]))
                    {
                        RefreshNullEquipment(i);
                        continue;
                    }
                    var gemDynamicData = Package_Gem.Instance.GetGemDynamicDataByUID(currentPlayer.gems[i]);
                    if (gemDynamicData != null)
                    {
                        equipment.GetChild(i).GetComponent<UI_Package_Item>().Refresh(gemDynamicData.rarity
                        , Package_Gem.Instance.GetGemDataByID(gemDynamicData.id).ItemIcon
                        , packagePlane, Package_Gem.Instance.GetGemInPackageIndex(gemDynamicData)
                        , 1, true, currentPlayer.playerEquipment);
                    }
                    else
                    {
                        RefreshNullEquipment(i);
                    }
                }
                RefreshEquipmentChild(currentPlayer.equipmentGemCount);
                break;
            case PackageType.Accessories:
                for (int i = 0; i < currentPlayer.accessories.Count; i++)
                {
                    if (string.IsNullOrEmpty(currentPlayer.accessories[i]))
                    {
                        RefreshNullEquipment(i);
                        continue;
                    }
                    var accessoriesDynamicData = Package_Accessories.Instance.GetAccessoriesDynamicDataByUID(currentPlayer.accessories[i]);
                    if (accessoriesDynamicData != null)
                    {
                        equipment.GetChild(i).GetComponent<UI_Package_Item>().Refresh(accessoriesDynamicData.rarity
                        , Package_Accessories.Instance.GetAccessoriesDataByID(accessoriesDynamicData.id).ItemIcon
                        , packagePlane, Package_Accessories.Instance.GetAccessoriesInPackageIndex(accessoriesDynamicData)
                        , 1, true, currentPlayer.playerEquipment);
                    }
                    else
                    {
                        RefreshNullEquipment(i);
                    }
                }
                RefreshEquipmentChild(currentPlayer.equipmentAccessoriesCount);
                break;
        }

        RefreshAllItemMask();
    }

    private void RefreshNullEquipment(int index)
    {
        packagePlane.RefreshNullItemIcon(equipment, index);
    }

    private void RefreshEquipmentChild(int count)
    {
        for (int i = 0; i < equipment.childCount; i++)
        {
            if (i < count)
            {
                equipment.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                equipment.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    
    public void RefreshAllItemMask()
    {
        foreach (var child in equipment.GetComponentsInChildren<UI_Package_Item>())
        {
            child.RefreshMask(false);
        }
    }

    public void RefreshAllItemMask(int ignoreIndex)
    {
        foreach (var child in equipment.GetComponentsInChildren<UI_Package_Item>())
        {
            child.RefreshMask(true);
        }
        equipment.GetChild(ignoreIndex).GetComponent<UI_Package_Item>().RefreshMask(false);
    }

    private void ChangeEquipmentIndex(int index)
    {
        equipmentIndex = index;
        RefreshAllItemMask(index);
        if (packageIndex != -1) EquipmentItem();
    }

    private void ChangePackageIndex(int index)
    {
        packageIndex = index;
        packagePlane.RefreshAllItemMask(index);
        if (equipmentIndex != -1) EquipmentItem();
    }

    public void EquipmentItem()
    {
        switch (currenPackage)
        {
            case PackageType.Item:
                var item = Package_Item.Instance.LoadPackage()[packageIndex];
                if (!item.isEquipment)
                {
                    var currentEquipmentId = currentPlayer.items[equipmentIndex];
                    if (!string.IsNullOrEmpty(currentEquipmentId))
                    {
                        var currentEquipmentItem = Package_Item.Instance.GetItemDynamicDataByID(currentEquipmentId);
                        if (currentEquipmentItem != null) currentEquipmentItem.isEquipment = false;
                    }
                    item.isEquipment = true;
                    currentPlayer.EquipmentItem(PackageType.Item, item.id, equipmentIndex);
                }
                RefreshPackage();
                break;
            case PackageType.Weapon:
                var weapon = Package_Weapon.Instance.LoadPackage()[packageIndex];
                if (!weapon.isEquipment)
                {
                    var currentEquipmentUid = currentPlayer.weapons[equipmentIndex];
                    if (!string.IsNullOrEmpty(currentEquipmentUid))
                    {
                        var currentEquipmentWeapon = Package_Weapon.Instance.GetWeaponDynamicDataByUID(currentEquipmentUid);
                        if (currentEquipmentWeapon != null) currentEquipmentWeapon.isEquipment = false;
                    }
                    weapon.isEquipment = true;
                    currentPlayer.EquipmentItem(PackageType.Weapon, weapon.uid, equipmentIndex);
                }
                RefreshPackage();
                break;
            case PackageType.Gem:
                var gem = Package_Gem.Instance.LoadPackage()[packageIndex];
                if (!gem.isEquipment)
                {
                    var currentEquipmentUid = currentPlayer.gems[equipmentIndex];
                    if (!string.IsNullOrEmpty(currentEquipmentUid))
                    {
                        var currentEquipmentGem = Package_Gem.Instance.GetGemDynamicDataByUID(currentEquipmentUid);
                        if (currentEquipmentGem != null) currentEquipmentGem.isEquipment = false;
                    }
                    gem.isEquipment = true;
                    currentPlayer.EquipmentItem(PackageType.Gem, gem.uid, equipmentIndex);
                }
                RefreshPackage();
                break;
            case PackageType.Accessories:
                var accessories = Package_Accessories.Instance.LoadPackage()[packageIndex];
                if (!accessories.isEquipment)
                {
                    var currentEquipmentUid = currentPlayer.accessories[equipmentIndex];
                    if (!string.IsNullOrEmpty(currentEquipmentUid))
                    {
                        var currentEquipmentAccessories = Package_Accessories.Instance.GetAccessoriesDynamicDataByUID(currentEquipmentUid);
                        if (currentEquipmentAccessories != null) currentEquipmentAccessories.isEquipment = false;
                    }
                    accessories.isEquipment = true;
                    currentPlayer.EquipmentItem(PackageType.Accessories, accessories.uid, equipmentIndex);
                }
                RefreshPackage();
                break;
        }
        equipmentIndex = -1;
        packageIndex = -1;
        RefreshAllItemMask();
        packagePlane.RefreshAllItemMask();
    }
}
