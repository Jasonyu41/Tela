using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Attack_1", fileName = "Boss_1_Attack_1")]
public class Boss_1_Attack_1 : BossState
{
    [Header("State Setting")]
    [SerializeField] Vector3 moveInput;
    [SerializeField] float directReleaseDistance;
    [SerializeField] float waitStartAttackTime;
    [SerializeField] Vector3 attackVFXOffset;

    WaitForSeconds waitForStartAttackTime;

    protected override void OnEnable()
    {
        base.OnEnable();

        waitForStartAttackTime = new WaitForSeconds(waitStartAttackTime);
    }

    public override void Enter()
    {
        base.Enter();
        
        bossController.SetInputs(moveInput);
        bossController.StartCoroutine(Attack());
    }
    
    IEnumerator Attack()
    {
        yield return waitForStartAttackTime;

        if (!boss.GetComponent<Boss_1>().isBreak)
        {
            bossController.SetInputs(Vector3.zero);
            
            if ((playerTransform.position - bossTransform.position).magnitude <= directReleaseDistance)
            {
                PoolManager.Release(attackVFX, bossTransform.TransformPoint(attackVFXOffset));
            }
            else
            {
                var temp = playerTransform.position + playerTransform.forward;
                temp.y = bossTransform.position.y;
                bossController.motor.SetPosition(temp);
                PoolManager.Release(attackVFX, bossTransform.TransformPoint(attackVFXOffset));
            }

            ReturnBaseState();
        }
    }
}
