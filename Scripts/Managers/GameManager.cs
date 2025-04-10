using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("---- Player ----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] CinemachineInputProvider cinemachineInputProvider;
    [SerializeField] public bool canLookAtBoss;
    [SerializeField] public bool isLookAtEnemy;
    [HideInInspector] public int money; // max value 1,919,114,514

    [Header("---- UI ----")]
    [SerializeField] GameObject UI;
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] UI_Pause UI_Pause;
    [SerializeField] UI_OperateInfo UI_OperateInfo;

    [Header("---- Items Data ----")]
    public Sprite[] rarity;
    public ItemData[] itemDatas;
    public WeaponData[] weaponDatas;
    public GemData[] gemDatas;
    public AccessoriesData[] accessoriesDatas;

    [Header("---- Misc ----")]
    [SerializeField] StartMovie startMovie;

    const string GAME_DATA_NAME = "GameData.sav";
    public bool isFirstInGame;


    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnPause += UnPause;
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnPause -= UnPause;
    }

    private void Start()
    {

        if (LoadSave())
        {
            UI_Pause.gameObject.SetActive(true);
            UnPause();

            startMovie.gameObject.SetActive(false);
            // Destroy(startMovie.gameObject);
        }
        else
        {
            // 第一次進遊戲
            cinemachineInputProvider.enabled = false;
            hUDCanvas.enabled = false;
            UI.SetActive(true);
            UI_Pause.gameObject.SetActive(true);
            UI_Pause.gameObject.SetActive(false);
            playerInput.DisableAllInputs();

            FirstGame();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        cinemachineInputProvider.enabled = false;
        hUDCanvas.enabled = false;
        UI.SetActive(true);
        UI_Pause.gameObject.SetActive(true);
        playerInput.EnableUIInput();
        playerInput.SwitchToDynamicUpdateMode();
    }
    public void Pause(bool isOpenUIPause = true)
    {
        Pause();
        
        UI_Pause.gameObject.SetActive(isOpenUIPause);
    }
    public void UnPause()
    {
        if (UI_OperateInfo.gameObject.activeSelf)
        {
            UI_OperateInfo.gameObject.SetActive(false);
        }
        Time.timeScale = 1;
        cinemachineInputProvider.enabled = true;
        hUDCanvas.enabled = true;
        UI.SetActive(false);
        UI.SetActive(true);
        UI_Pause.gameObject.SetActive(false);
        playerInput.EnableGameplayInputs();
        playerInput.SwitchToFixedUpdateMode();
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.player01Equipment.items = PartyManager.party[0].items;
        saveData.player01Equipment.weapons = PartyManager.party[0].weapons;
        saveData.player01Equipment.gems = PartyManager.party[0].gems;
        saveData.player01Equipment.accessories = PartyManager.party[0].accessories;

        saveData.player02Equipment.items = PartyManager.party[1].items;
        saveData.player02Equipment.weapons = PartyManager.party[1].weapons;
        saveData.player02Equipment.gems = PartyManager.party[1].gems;
        saveData.player02Equipment.accessories = PartyManager.party[1].accessories;

        saveData.player03Equipment.items = PartyManager.party[2].items;
        saveData.player03Equipment.weapons = PartyManager.party[2].weapons;
        saveData.player03Equipment.gems = PartyManager.party[2].gems;
        saveData.player03Equipment.accessories = PartyManager.party[2].accessories;
        
        saveData.packageItem = Package_Item.Instance.LoadPackage();
        saveData.packageWeapon = Package_Weapon.Instance.LoadPackage();
        saveData.packageGem = Package_Gem.Instance.LoadPackage();
        saveData.packageAccessories = Package_Accessories.Instance.LoadPackage();

        SaveManager.SaveByJson(GAME_DATA_NAME, saveData, true);
    }

    public bool LoadSave()
    {
        SaveData saveData = SaveManager.LoadFromJson<SaveData>(GAME_DATA_NAME);
        saveData = null;
        if (saveData != null)
        {
            isFirstInGame = false;

            // party member sort
            PartyManager.party[0].items = saveData.player01Equipment.items;
            PartyManager.party[0].weapons = saveData.player01Equipment.weapons;
            PartyManager.party[0].gems = saveData.player01Equipment.gems;
            PartyManager.party[0].accessories = saveData.player01Equipment.accessories;

            PartyManager.party[1].items = saveData.player02Equipment.items;
            PartyManager.party[1].weapons = saveData.player02Equipment.weapons;
            PartyManager.party[1].gems = saveData.player02Equipment.gems;
            PartyManager.party[1].accessories = saveData.player02Equipment.accessories;

            PartyManager.party[2].items = saveData.player03Equipment.items;
            PartyManager.party[2].weapons = saveData.player03Equipment.weapons;
            PartyManager.party[2].gems = saveData.player03Equipment.gems;
            PartyManager.party[2].accessories = saveData.player03Equipment.accessories;

            Package_Item.Instance.package = saveData.packageItem;
            Package_Weapon.Instance.package = saveData.packageWeapon;
            Package_Gem.Instance.package = saveData.packageGem;
            Package_Accessories.Instance.package = saveData.packageAccessories;

            return true;
        }

        return false;
    }

#region First Game
    private void FirstGame()
    {
        isFirstInGame = true;

        money = 5140;
        Package_Item.Instance.package = null;
        Package_Weapon.Instance.package = null;
        Package_Gem.Instance.package = null;
        Package_Accessories.Instance.package = null;
        InitialItem();
        InitialWeapon();
        InitialGem();
        InitialAccessories();

        startMovie.gameObject.SetActive(true);
        startMovie.Play();
    }

    private void InitialItem()
    {
        Package_Item.Instance.CreatItem("I_01", 0, 1);
    }

    private void InitialWeapon()
    {
        Package_Weapon.Instance.CreatWeapon("W_01", 1, true);
        Package_Weapon.Instance.CreatWeapon("W_02", 3, true, 1);
        Package_Weapon.Instance.CreatWeapon("W_01", 2, false);
        Package_Weapon.Instance.CreatWeapon("W_02", 4, false);
    }

    private void InitialGem()
    {
        Package_Gem.Instance.CreatGem("G_01_01", 0, true);
        Package_Gem.Instance.CreatGem("G_01_02", 0, true, 1);
        Package_Gem.Instance.CreatGem("G_01_03", 0);
    }

    private void InitialAccessories()
    {
        Package_Accessories.Instance.CreatAccessories("A_13", 0, true);
        Package_Accessories.Instance.CreatAccessories("A_16", 1, true, 1, 1);
        Package_Accessories.Instance.CreatAccessories("A_17", 2);
    }
#endregion

}

class SaveData
{
    public PlayerSaveData player01Equipment;
    public PlayerSaveData player02Equipment;
    public PlayerSaveData player03Equipment;
    public List<ItemDynamicData> packageItem;
    public List<WeaponDynamicData> packageWeapon;
    public List<GemDynamicData> packageGem;
    public List<AccessoriesDynamicData> packageAccessories;
}

[Serializable]
struct PlayerSaveData
{
    public List<string> items;
    public List<string> weapons;
    public List<string> gems;
    public List<string> accessories;
}