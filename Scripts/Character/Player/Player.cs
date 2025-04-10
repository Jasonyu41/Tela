using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Player : Character
{
    [Header("----Player Stats----")]
    [SerializeField] public string playerName;
    [SerializeField] public Sprite playerIcon;
    [SerializeField] public Sprite playerSubIcon;
    [SerializeField] public Sprite playerBanner;
    [SerializeField] public Sprite playerEquipment;

    [Header("----Player Equipment----")]
    [SerializeField] public int equipmentItemCount = 2;
    [SerializeField] public int equipmentWeaponCount = 1;
    [SerializeField] public int equipmentGemCount = 2;
    [SerializeField] public int equipmentAccessoriesCount = 3;
    [SerializeField] WeaponStaticData[] equipmentWeapon;
    [SerializeField] DamageType equipmentWeaponDamageType;
    [SerializeField] GemStaticData[] equipmentGem;
    [SerializeField] AccessoriesStaticData[] equipmentAccessories;
    public List<string> items; //id
    public List<string> weapons; //uid
    public List<string> gems; //uid
    public List<string> accessories; //uid

    [Header("----Player Setting----")]
    [SerializeField] float takeDamegePauseGameFrame = 5f;

    [Header("TEMP")]
    [SerializeField] public CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] public CinemachineFreeLook cinLookAt;

    protected override void Awake()
    {
        base.Awake();

        equipmentWeapon = new WeaponStaticData[equipmentWeaponCount];
        equipmentGem = new GemStaticData[equipmentGemCount];
        equipmentAccessories = new AccessoriesStaticData[equipmentAccessoriesCount];

        InitinlizePackageList(ref items, equipmentItemCount);
        InitinlizePackageList(ref weapons, equipmentWeaponCount);
        InitinlizePackageList(ref gems, equipmentGemCount);
        InitinlizePackageList(ref accessories, equipmentAccessoriesCount);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.O))
        {
            RestoreHealth(100);
        }
    }

    public override void TakeDamege(float value)
    {
        base.TakeDamege(value);
        
        TimeManager.PauseGame(takeDamegePauseGameFrame, true);
        // Cinmachine Shack
        
        UI_HUD.UpdatePartyStatsBar();
    }

    public override void Die()
    {
        base.Die();
    }

    private void InitinlizePackageList(ref List<string> package, int packageCount)
    {
        package = new(packageCount);

        for (int i = 0; i < packageCount; i++)
        {
            package.Add(null);
        }
    }

    public void EquipmentItem(PackageType itemType, string uid, int index)
    {
        switch (itemType)
        {
            case PackageType.Item:
                items[index] = uid;
                break;
            case PackageType.Weapon:
                if (!string.IsNullOrEmpty(weapons[index]))
                {
                    characterData.maxHealth -= equipmentWeapon[index].health;
                    characterData.health -= equipmentWeapon[index].health;
                    if (equipmentWeaponDamageType == DamageType.PhysicalDamage) characterData.physicalAttack -= equipmentWeapon[index].attack;
                    if (equipmentWeaponDamageType == DamageType.MagicDamage) characterData.magicAttack -= equipmentWeapon[index].attack;
                    characterData.criticalHit -= equipmentWeapon[index].criticalHit;
                    characterData.attribute.fireAttack -= equipmentWeapon[index].fireAttack;
                    characterData.attribute.woodenAttack -= equipmentWeapon[index].woodenAttack;
                    characterData.attribute.waterAttack -= equipmentWeapon[index].waterAttack;
                    characterData.attribute.rockAttack -= equipmentWeapon[index].rockAttack;
                    characterData.attribute.electricAttack -= equipmentWeapon[index].electricAttack;
                    characterData.attribute.darkAttack -= equipmentWeapon[index].darkAttack;
                }
                weapons[index] = uid;
                var weaponDynamicData = Package_Weapon.Instance.GetWeaponDynamicDataByUID(uid);
                var weaponData = Package_Weapon.Instance.GetWeaponDataByID(weaponDynamicData.id);
                var weapon = weaponData.weaponStaticDataList[weaponDynamicData.level - 1];
                equipmentWeapon[index] = weapon;
                equipmentWeaponDamageType = weaponData.damageType;

                characterData.maxHealth += weapon.health;
                characterData.health += weapon.health;
                if (weaponData.damageType == DamageType.PhysicalDamage) characterData.physicalAttack += weapon.attack;
                if (weaponData.damageType == DamageType.MagicDamage) characterData.magicAttack += weapon.attack;
                characterData.criticalHit += weapon.criticalHit;
                characterData.attribute.fireAttack += weapon.fireAttack;
                characterData.attribute.woodenAttack += weapon.woodenAttack;
                characterData.attribute.waterAttack += weapon.waterAttack;
                characterData.attribute.rockAttack += weapon.rockAttack;
                characterData.attribute.electricAttack += weapon.electricAttack;
                characterData.attribute.darkAttack += weapon.darkAttack;
                break;
            case PackageType.Gem:
                if (!string.IsNullOrEmpty(gems[index]))
                {
                    characterData.maxHealth -= equipmentGem[index].health;
                    characterData.health -= equipmentGem[index].health;
                    characterData.physicalDefense -= equipmentGem[index].physicalDefense;
                    characterData.magicDefense -= equipmentGem[index].magicDefense;
                    characterData.attribute.fireDefense -= equipmentGem[index].fireDefense;
                    characterData.attribute.woodenDefense -= equipmentGem[index].woodenDefense;
                    characterData.attribute.waterDefense -= equipmentGem[index].waterDefense;
                    characterData.attribute.rockDefense -= equipmentGem[index].rockDefense;
                    characterData.attribute.electricDefense -= equipmentGem[index].electricDefense;
                    characterData.attribute.darkDefense -= equipmentGem[index].darkDefense;
                }
                gems[index] = uid;
                var gemDynamicData = Package_Gem.Instance.GetGemDynamicDataByUID(uid);
                var gem = Package_Gem.Instance.GetGemStaticDataByID(gemDynamicData.id, gemDynamicData.rarity);
                equipmentGem[index] = gem;

                characterData.maxHealth += gem.health;
                characterData.health += gem.health;
                characterData.physicalDefense += gem.physicalDefense;
                characterData.magicDefense += gem.magicDefense;
                characterData.attribute.fireDefense += gem.fireDefense;
                characterData.attribute.woodenDefense += gem.woodenDefense;
                characterData.attribute.waterDefense += gem.waterDefense;
                characterData.attribute.rockDefense += gem.rockDefense;
                characterData.attribute.electricDefense += gem.electricDefense;
                characterData.attribute.darkDefense += gem.darkDefense;
                break;
            case PackageType.Accessories:
                if (!string.IsNullOrEmpty(accessories[index]))
                {
                    characterData.criticalHit -= equipmentAccessories[index].criticalHit;
                    characterData.criticalHitDamage -= equipmentAccessories[index].criticalHitDamage;
                    characterData.moveSpeed -= equipmentAccessories[index].moveSpeed;
                    characterData.attackSpeed -= equipmentAccessories[index].attackSpeed;
                    characterData.coolDown -= equipmentAccessories[index].coolDown;
                    characterData.luck -= equipmentAccessories[index].luck;
                }
                accessories[index] = uid;
                var accessoriesDynamicData = Package_Accessories.Instance.GetAccessoriesDynamicDataByUID(uid);
                var accessorie = Package_Accessories.Instance.GetAccessoriesStaticDataByID(accessoriesDynamicData.id, accessoriesDynamicData.rarity);
                equipmentAccessories[index] = accessorie;

                characterData.criticalHit += accessorie.criticalHit;
                characterData.criticalHitDamage += accessorie.criticalHitDamage;
                characterData.moveSpeed += accessorie.moveSpeed;
                characterData.attackSpeed += accessorie.attackSpeed;
                characterData.coolDown += accessorie.coolDown;
                characterData.luck += accessorie.luck;
                break;
        }
        // Refresh Now Item UI And Object
    }

    public void PlayVFX(GameObject vfx)
    {
        AttackData physicalAttack = new();
        AttackData magicAttack = new();
        AttackData attribute = new();

        physicalAttack.damage = characterData.physicalAttack;
        physicalAttack.damageAddition = characterData.physicalAttackAddition;
        physicalAttack.damageType = DamageType.PhysicalDamage;
        physicalAttack.criticalHit = characterData.criticalHit;
        physicalAttack.criticalHitDamage = characterData.criticalHitDamage;

        magicAttack.damage = characterData.magicAttack;
        magicAttack.damageAddition = characterData.magicAttackAddition;
        magicAttack.damageType = DamageType.MagicDamage;
        magicAttack.criticalHit = characterData.criticalHit;
        magicAttack.criticalHitDamage = characterData.criticalHitDamage;

        switch (characterData.selfAttribute)
        {
            case AttributeType.Fire:
                attribute.damage = characterData.attribute.fireAttack;
                attribute.damageAddition = characterData.attribute.fireAttackAddition;
                break;
            case AttributeType.Wooden:
                attribute.damage = characterData.attribute.woodenAttack;
                attribute.damageAddition = characterData.attribute.woodenAttackAddition;
                break;
            case AttributeType.Water:
                attribute.damage = characterData.attribute.waterAttack;
                attribute.damageAddition = characterData.attribute.waterAttackAddition;
                break;
            case AttributeType.Rock:
                attribute.damage = characterData.attribute.rockAttack;
                attribute.damageAddition = characterData.attribute.rockAttackAddition;
                break;
            case AttributeType.Electric:
                attribute.damage = characterData.attribute.electricAttack;
                attribute.damageAddition = characterData.attribute.electricAttackAddition;
                break;
            case AttributeType.Dark:
                attribute.damage = characterData.attribute.darkAttack;
                attribute.damageAddition = characterData.attribute.darkAttackAddition;
                break;
        }

        attribute.damageType = DamageType.AttributeDamage;
        attribute.criticalHit = characterData.criticalHit;
        attribute.criticalHitDamage = characterData.criticalHitDamage;

        PoolManager.Release(vfx, tf).GetComponent<PlayerAttack>().Initinlize(physicalAttack, magicAttack, attribute);
    }
}
