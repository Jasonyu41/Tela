using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("---- Boss Stats ----")]
    [SerializeField, Range(0f, 1f)] public float enterSecondStageHP = 0.5f;
    [SerializeField] Canvas bossCanvas;
    [SerializeField] UI_StatsBar HPBar;
    
    protected BossController bossController;

    protected override void Awake()
    {
        base.Awake();

        bossController = GetComponent<BossController>();

        bossCanvas.enabled = false;
    }

    public virtual void EnterBattle()
    {
        HPBar.Initialize(characterData.health, characterData.maxHealth);
        bossCanvas.enabled = true;
        bossController.EnterBattle();
    }

    public override void TakeDamege(float value)
    {
        base.TakeDamege(value);

        HPBar.UpdateStats(characterData.health, characterData.maxHealth);
    }

    public override void Die()
    {
        base.Die();

        bossCanvas.enabled = false;
        PartyManager.party[0].cinLookAt.Priority = 5;
        PartyManager.party[1].cinLookAt.Priority = 5;
        PartyManager.party[2].cinLookAt.Priority = 5;
        GameManager.Instance.isLookAtEnemy = false;
        GameManager.Instance.canLookAtBoss = false;
    }
}