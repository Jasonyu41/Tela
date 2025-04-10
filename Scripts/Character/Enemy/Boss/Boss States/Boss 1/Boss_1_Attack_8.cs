using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Attack_8", fileName = "Boss_1_Attack_8")]
public class Boss_1_Attack_8 : BossState
{
    [Header("State Setting")]
    [SerializeField] Vector3 attackVFXOffset;

    public override void Enter()
    {
        base.Enter();
        
        PoolManager.Release(attackVFX, bossTransform.TransformPoint(attackVFXOffset));
    }

    public override void Update()
    {
        base.Update();
        
        if (IsAnimationFinished)
        {
            ReturnBaseState();
        }
    }
}
