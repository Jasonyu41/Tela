using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Attack_VFX_3 : Boss_1_Attack_VFX
{
    [SerializeField] float waitAttackTime;
    [SerializeField] float colliderDurationTime;

    WaitForSeconds waitForAttackTime;
    WaitForSeconds waitForColliderDurationTime;

    protected override void Awake()
    {
        base.Awake();
        
        waitForAttackTime = new WaitForSeconds(waitAttackTime);
        waitForColliderDurationTime = new WaitForSeconds(colliderDurationTime);
    }

    protected override void OnEnable()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return waitForAttackTime;

        base.OnEnable();

        yield return waitForColliderDurationTime;

        base.OnDisable();
    }
}
