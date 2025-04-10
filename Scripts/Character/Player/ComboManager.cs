using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] List<ComboConfig> skillComboConfig = new();
    [SerializeField] WeaponConfig weaponConfig;
    // [SerializeField] float releaseTime;

    float releaseTimer;
    [HideInInspector] public bool isComboing;
    int beforeComboType;        //ComboType Light=0 Mid=1 Heavy=2 LastAttack=3
    int lightAttackIndex = 0;
    int midAttackIndex = 0;
    int heavyAttackIndex = 0;
    const float animationFadeTime = 0.0f;
    Animator animator;
    Transform tf;
    PlayerCharacterController playerCharacterController;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        tf = transform;
        playerCharacterController = GetComponent<PlayerCharacterController>();
        StopCombo();
    }

    private void OnEnable()
    {
        playerInput.onLightAttack += LightAttack;
        playerInput.onHeavyAttack += HeavyAttack;
        releaseTimer = 0;
        isComboing = false;
        lightAttackIndex = 0;
        midAttackIndex = 0;
        heavyAttackIndex = 0;
    }

    private void OnDisable()
    {
        playerInput.onLightAttack -= LightAttack;
        playerInput.onHeavyAttack -= HeavyAttack;
    }

#region Player Input
    private void LightAttack()
    {
        if (Time.time > releaseTimer)
        {
            StopCombo();
        }

        if (isComboing)
        {
            return;
        }

        if (beforeComboType == 1)
        {
            Attack(1);
        }
        else
        {
            Attack(0);
        }
    }

    private void HeavyAttack()
    {
        if (Time.time > releaseTimer)
        {
            StopCombo();
        }

        if (isComboing)
        {
            return;
        }

        if (playerInput.isFrontSkillDown)
        {
            Skill(0);
        }
        else if (playerInput.isBackSkillDown)
        {
            Skill(3);
        }
        else if (playerInput.isLeftSkillDown)
        {
            Skill(1);
        }
        else if (playerInput.isRightSkillDown)
        {
            Skill(2);
        }
        else
        {
            if (beforeComboType == 0 || beforeComboType == 1)
            {
                Attack(1);
            }
            else
            {
                Attack(2);
            }
        }
    }
#endregion

    private void Attack(int attackType)
    {
        List<ComboConfig> configs = new List<ComboConfig>();
        int comboIndex = 0;
        switch (attackType)
        {
            case 0:
                configs = weaponConfig.lightComboConfigs;
                comboIndex = lightAttackIndex;
                beforeComboType = 0;
                break;
            case 1:
                configs = weaponConfig.midComboConfigs;
                comboIndex = midAttackIndex;
                beforeComboType = 1;
                break;
            case 2:
                configs = weaponConfig.heavyComboConfigs;
                comboIndex = heavyAttackIndex;
                beforeComboType = 2;
                break;
        }

        StartCoroutine(PlayCombo(configs[comboIndex]));

        if (comboIndex >= configs.Count - 1)
        {
            comboIndex = 0;
            beforeComboType = 3;
        }
        else
        {
            comboIndex++;
        }

        switch (attackType)
        {
            case 0:
                lightAttackIndex = comboIndex;
                midAttackIndex = 0;
                heavyAttackIndex = 0;
                break;
            case 1:
                lightAttackIndex = 0;
                midAttackIndex = comboIndex;
                heavyAttackIndex = 0;
                break;
            case 2:
                lightAttackIndex = 0;
                midAttackIndex = 0;
                heavyAttackIndex = comboIndex;
                break;
        }
    }

    public void Skill(int skillIndex)
    {
        if (isComboing) return;

        StartCoroutine(PlayCombo(skillComboConfig[skillIndex]));
    }

    IEnumerator PlayCombo(ComboConfig comboConfig)
    {
        isComboing = true;
        releaseTimer = Time.time + comboConfig.releaseTime;

        animator.CrossFade(comboConfig.animationName, comboConfig.animationFadeTime);

        yield return new WaitForSeconds(comboConfig.durationTime);

        isComboing = false;;
    }
    
    private void StopCombo()
    {
        lightAttackIndex = 0;
        midAttackIndex = 0;
        heavyAttackIndex = 0;
        beforeComboType = 3;
    }
}
