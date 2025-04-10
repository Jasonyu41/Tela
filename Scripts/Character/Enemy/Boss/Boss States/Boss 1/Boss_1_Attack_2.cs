using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Attack_2", fileName = "Boss_1_Attack_2")]
public class Boss_1_Attack_2 : BossState
{
    [Header("State Setting")]
    [SerializeField] Vector3 moveInput;
    [SerializeField] float waitStartAttackTime;
    [SerializeField] int bulletHorizontalCount = 1;
    [SerializeField] int bulletVerticalCount = 1;
    [SerializeField] int shootCount = 1;
    [SerializeField] Vector3 selfPositionOffset;
    [SerializeField] Vector3 targetPositionOffset;
    [SerializeField] Vector3 bulletHorizontalSpacing;
    [SerializeField] Vector3 bulletVerticalSpacing;
    [SerializeField] float bulletHorizontalAngle;
    [SerializeField] float bulletVerticalAngle;
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

        bossController.SetInputs(moveInput);

        for (int i = 0; i < shootCount; i++)
        {
            dir = playerTransform.TransformPoint(targetPositionOffset) - bossTransform.TransformPoint(selfPositionOffset);
            angle = Vector3.Angle(dir, bossTransform.forward);

            for (int m = 0; m < bulletHorizontalCount; m++)
            {
                for (int n = 0; n < bulletVerticalCount; n++)
                {
                    PoolManager.Release(attackVFX
                                        , bossTransform.TransformPoint(selfPositionOffset 
                                            - ((bulletHorizontalCount - 1f) / 2f - m) * bulletHorizontalSpacing
                                            - ((bulletVerticalCount - 1f) / 2f - n) * bulletVerticalSpacing)
                                        , bossController.motor.TransientRotation * Quaternion.Euler(angle 
                                            + ((bulletVerticalCount - 1f) / 2f - n) * bulletVerticalAngle, 
                                            - ((bulletHorizontalCount - 1f) / 2f - m) * bulletHorizontalAngle, 0));
                }
            }
            yield return waitForShootIntervalTime;
        }

        isAttackFinished = true;
    }
}
