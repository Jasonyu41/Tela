using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HUD : Singleton<UI_HUD>
{
    [SerializeField] TextMeshProUGUI member1_Name;
    [SerializeField] Image member1_Icon;
    [SerializeField] Image member2_Icon;
    [SerializeField] Image member3_Icon;
    [SerializeField] UI_StatsBar member1_HPBar;
    [SerializeField] UI_StatsBar member1_MPBar;
    [SerializeField] UI_StatsBar member2_HPBar;
    [SerializeField] UI_StatsBar member3_HPBar;
    
    string[] partyName = new string[3];
    Sprite[] partyIcon = new Sprite[3];
    Sprite[] partySubIcon = new Sprite[3];

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            partyName[i] = PartyManager.party[i].playerName;
            partyIcon[i] = PartyManager.party[i].playerIcon;
            partySubIcon[i] = PartyManager.party[i].playerSubIcon;
        }

        Instance.member1_Name.text = Instance.partyName[0];
        Instance.member1_Icon.sprite = Instance.partyIcon[0];
        Instance.member2_Icon.sprite = Instance.partySubIcon[1];
        Instance.member3_Icon.sprite = Instance.partySubIcon[2];
        member1_HPBar.Initialize(PartyManager.party[0].characterData.health, PartyManager.party[0].characterData.maxHealth);
        member1_MPBar.Initialize(PartyManager.party[0].characterData.manaPoint, PartyManager.party[0].characterData.maxManaPoint);
        member2_HPBar.Initialize(PartyManager.party[1].characterData.health, PartyManager.party[0].characterData.maxHealth);
        member3_HPBar.Initialize(PartyManager.party[2].characterData.health, PartyManager.party[0].characterData.maxHealth);
    }

    public static void SwitchCharacterStats(int index)
    {
        (Instance.partyName[0], Instance.partyName[index]) = (Instance.partyName[index], Instance.partyName[0]);
        (Instance.partyIcon[0], Instance.partyIcon[index]) = (Instance.partyIcon[index], Instance.partyIcon[0]);
        (Instance.partySubIcon[0], Instance.partySubIcon[index]) = (Instance.partySubIcon[index], Instance.partySubIcon[0]);

        Instance.member1_Name.text = Instance.partyName[0];
        Instance.member1_Icon.sprite = Instance.partyIcon[0];
        if (index == 1)
        {
            Instance.member2_Icon.sprite = Instance.partySubIcon[index];
        }
        else
        {
            Instance.member3_Icon.sprite = Instance.partySubIcon[index];
        }

        UpdatePartyStatsBar();
    }

    public static void UpdatePartyStatsBar()
    {
        Instance.member1_HPBar.UpdateStats(PartyManager.party[0].characterData.health, PartyManager.party[0].characterData.maxHealth);
        Instance.member1_MPBar.UpdateStats(PartyManager.party[0].characterData.manaPoint, PartyManager.party[0].characterData.maxManaPoint);
        Instance.member2_HPBar.UpdateStats(PartyManager.party[1].characterData.health, PartyManager.party[0].characterData.maxHealth);
        Instance.member3_HPBar.UpdateStats(PartyManager.party[2].characterData.health, PartyManager.party[0].characterData.maxHealth);
    }
}
