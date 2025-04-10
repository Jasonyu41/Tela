using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Attack_7", fileName = "Boss_1_Attack_7")]
public class Boss_1_Attack_7 : BossState
{
    [Header("State Setting")]
    [SerializeField] Vector3 attackVFXOffset;
    [SerializeField] string switchToStateName = "Boss_1_Attack_8";
    [SerializeField] VoidEventChannel isHitPlayerEventChannel;

    bool isHitPlayer;

    protected override void OnEnable()
    {
        base.OnEnable();

        isHitPlayerEventChannel.AddListener(SwitchIsHitPlayer);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        isHitPlayerEventChannel.RemoveListener(SwitchIsHitPlayer);
    }

    public override void Enter()
    {
        base.Enter();
        
        isHitPlayer = false;
        PoolManager.Release(attackVFX, bossTransform.TransformPoint(attackVFXOffset));
    }

    public override void Update()
    {
        base.Update();
        
        if (IsAnimationFinished)
        {
            if (isHitPlayer)
            {
                bossController.SwitchState(switchToStateName);
            }
            else
            {
                ReturnBaseState();
            }
        }
    }

    void SwitchIsHitPlayer()
    {
        isHitPlayer = true;
    }
}
