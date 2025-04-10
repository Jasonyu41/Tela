using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : Boss
{
    [Header("---- Boss 01 Stats ----")]
    [SerializeField] Boss_1_Shield shieldVFX;
    [SerializeField] Vector3 shedldHitOffset;
    [SerializeField] float shield;
    [SerializeField] float maxShield;
    [SerializeField, Range(0, 100)] float shieldDefense;
    [SerializeField] float shieldAddition;
    [SerializeField] float shieldBreakDamege;
    [SerializeField] float shieldRestoreTime;
    [SerializeField] UI_StatsBar shieldBar;

    [Header ("---- Animation ----")]
    [SerializeField] string shieldBreakTakeDamegeAnimation;

    [Header ("---- Misc ----")]
    [SerializeField] VoidEventChannel breakFinishedEventChannel;
    [SerializeField] GameObject frontRootDoor;
    [SerializeField] GameObject backRootDoor;
    [SerializeField] GameObject bossHDRITrigger;

    float shieldDamege;
    [HideInInspector] public bool isBreak;
    bool isShieldRestore;
    Boss_1_Controller boss_1_Controller;
    WaitForFixedUpdate waitForFixedUpdate;

    protected override void Awake()
    {
        base.Awake();

        boss_1_Controller = GetComponent<Boss_1_Controller>();
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        shield = maxShield;
        shieldBar.UpdateStats(shield, maxShield);
        breakFinishedEventChannel.AddListener(BreakFinished);
        frontRootDoor.SetActive(false);
    }
    
    private void OnDisable()
    {
        breakFinishedEventChannel.RemoveListener(BreakFinished);
    }

    public override void EnterBattle()
    {
        base.EnterBattle();
        
        frontRootDoor.SetActive(true);
        shieldBar.UpdateStats(shield, maxShield);
    }

    public override void TakeDamege(float value)
    {
        base.TakeDamege(ShieldDamege(value));
    }

    float ShieldDamege(float value)
    {
        if (shield <= 0 || isShieldRestore)
        {
            if (isBreak)
            {
                return value * (1 + shieldAddition / 100);
            }
            else
            {
                if (shieldBreakTakeDamegeAnimation != "")
                {
                    animator.Play(shieldBreakTakeDamegeAnimation);
                }
                return value;
            }
        }

        shieldDamege = value * shieldDefense / 100;
        shield -= shieldDamege;

        if (shield > 0)
        {
            shieldVFX.HitShield(transform.TransformPoint(shedldHitOffset));
        }
        else
        {
            shieldVFX.OpenCloseShield();
            TakeDamege(shieldBreakDamege);
            bossController.SetInputs(Vector3.zero);
            isBreak = true;
            boss_1_Controller.SwitchStateToBreak();
            shieldDamege += shield;
            shield = 0;
        }
        shieldBar.UpdateStats(shield, maxShield);
        return value - shieldDamege;
    }

    void BreakFinished()
    {
        isBreak = false;
        isShieldRestore = true;

        StartCoroutine(ShieldRestore());
    }

    IEnumerator ShieldRestore()
    {
        if (shieldRestoreTime != 0)
        {
            while(shield < maxShield)
            {
                shield += maxShield / shieldRestoreTime * Time.fixedDeltaTime;
                shieldBar.UpdateStats(shield, maxShield);
                yield return waitForFixedUpdate;
            }
        }
        shield = maxShield;
        shieldBar.UpdateStats(shield, maxShield);
        shieldVFX.OpenCloseShield();

        isShieldRestore = false;
    }

    public override void Die()
    {
        base.Die();

        frontRootDoor.SetActive(false);
        backRootDoor.SetActive(false);
        bossHDRITrigger.SetActive(false);
        EnvironmentManager.Instance.SwitchLightSetting(EnvironmentManager.Instance.defaultLight, 3);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamege(100);
        }
    }
}
