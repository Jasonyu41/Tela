using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Attack_6", fileName = "Boss_1_Attack_6")]
public class Boss_1_Attack_6 : BossState
{
    [Header("State Setting")]
    [SerializeField] Vector3 moveInput;
    [SerializeField] float waitStartAttackTime;
    [SerializeField] int shootCount = 1;
    [SerializeField] Vector3 selfPositionOffset;
    [SerializeField] Vector3 targetPositionOffset;
    [SerializeField] Vector3 shootSpacing;
    [SerializeField] float shootIntervalTime;

    Vector3 dir;
    float angle;
    bool isAttackFinished;

    WaitForSeconds waitForStartAttackTime;
    WaitForSeconds waitForShootIntervalTime;

    protected override void OnEnable()
    {
        base.OnEnable();

        waitForStartAttackTime = new WaitForSeconds(waitStartAttackTime);
        waitForShootIntervalTime = new WaitForSeconds(shootIntervalTime);
    }

    public override void Enter()
    {
        base.Enter();
        
        isAttackFinished = false;
        bossController.StartCoroutine(Attack());
    }

    public override void Update()
    {
        base.Update();

        if (isAttackFinished && IsAnimationFinished)
        {
            ReturnBaseState();
        }
    }

    IEnumerator Attack()
    {
        yield return waitForStartAttackTime;

        for (int i = 0; i < shootCount; i++)
        {
            dir = playerTransform.TransformPoint(targetPositionOffset) - bossTransform.TransformPoint(selfPositionOffset);
            angle = Vector3.Angle(dir, bossTransform.forward);

            PoolManager.Release(attackVFX, bossTransform.TransformPoint(selfPositionOffset + shootSpacing * i), bossController.motor.TransientRotation * Quaternion.Euler(angle, 0, 0));

            yield return waitForShootIntervalTime;
        }

        isAttackFinished = true;
    }
}
