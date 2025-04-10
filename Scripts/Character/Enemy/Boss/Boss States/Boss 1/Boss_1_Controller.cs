using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Controller : BossController
{
    [Header("---- Boss 01 Stats ----")]
    [SerializeField] Boss_1_Break boss_1_Break;

    Boss_1 boss_1;

    protected override void Awake()
    {
        base.Awake();

        boss_1_Break.Initialize(boss, this, playerTransform, animator);
        boss_1 = GetComponent<Boss_1>();
    }

    public override void SetInputs(Vector3 moveInput)
    {
        if (boss_1.isBreak) return;

        base.SetInputs(moveInput);
    }

    public void SwitchStateToBreak()
    {
        SwitchState(boss_1_Break);
    }

    public override void SwitchStateToDefault()
    {
        if (boss_1.isBreak) return;

        base.SwitchStateToDefault();
    }

    protected override void UpdatePlayerTransforme()
    {
        base.UpdatePlayerTransforme();

        boss_1_Break.UpdatePlayerTransform(playerTransform);
    }
}
