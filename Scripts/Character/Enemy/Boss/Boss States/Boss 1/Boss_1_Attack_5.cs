using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Attack_5", fileName = "Boss_1_Attack_5")]
public class Boss_1_Attack_5 : BossState
{
    [Header("State Setting")]
    [SerializeField] float waitStartAttackTime;
    [SerializeField] float rayDistance = 20f;
    [SerializeField] LayerMask rayLayerMask;
    
    RaycastHit hit;
    WaitForSeconds waitForStartAttackTime;

    protected override void OnEnable()
    {
        base.OnEnable();

        waitForStartAttackTime = new WaitForSeconds(waitStartAttackTime);
    }

    public override void Enter()
    {
        base.Enter();
        
        bossController.StartCoroutine(Attack());
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimationFinished)
        {
            ReturnBaseState();
        }
    }

    IEnumerator Attack()
    {
        yield return waitForStartAttackTime;

        Physics.Raycast(playerTransform.position, -playerTransform.up, out hit, rayDistance, rayLayerMask);
        PoolManager.Release(attackVFX, hit.point);
    }
}
