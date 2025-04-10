using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Attack_3", fileName = "Boss_1_Attack_3")]
public class Boss_1_Attack_3 : BossState
{
    [Header("State Setting")]
    [SerializeField] float waitStartAttackTime;
    [SerializeField] float attackIntervalTime;
    [SerializeField] Vector3 minOffset;
    [SerializeField] Vector3 maxOffset;
    [SerializeField] int bulletCount;
    [SerializeField] float rayDistance = 20f;
    [SerializeField] LayerMask rayLayerMask;
    
    Vector3 rndOffset = Vector3.zero;
    RaycastHit hit;

    WaitForSeconds waitForStartAttackTime;
    WaitForSeconds waitForAttackIntervalTime;

    protected override void OnEnable()
    {
        base.OnEnable();

        waitForStartAttackTime = new WaitForSeconds(waitStartAttackTime);
        waitForAttackIntervalTime = new WaitForSeconds(attackIntervalTime);
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

        for (int i = 0; i < bulletCount - 1; i++)
        {
            rndOffset.x = Random.Range(minOffset.x, maxOffset.x);
            rndOffset.z = Random.Range(minOffset.z, maxOffset.z);
            
            Physics.Raycast(playerTransform.position, -playerTransform.up, out hit, rayDistance, rayLayerMask);
            PoolManager.Release(attackVFX, hit.point + rndOffset);

            yield return waitForAttackIntervalTime;
        }

        Physics.Raycast(playerTransform.TransformPoint(playerTransform.forward), -playerTransform.up, out hit, rayDistance, rayLayerMask);
        PoolManager.Release(attackVFX, hit.point);
    }
}
