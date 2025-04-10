using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/SecondStage", fileName = "Boss_1_SecondStage")]
public class Boss_1_SecondStage : Boss_SecondStage
{
    [Header("State Setting")]
    [SerializeField] string secondAnimationName;
    [SerializeField] float transDuration = 0.1f;
    [SerializeField] float waitSecondAnimationTime = 2.74f;
    [SerializeField] Vector3 warpPosition;
    [SerializeField] Vector3 attackVFXOffset;

    float timer;
    
    WaitForFixedUpdate waitForFixedUpdate;


    protected override void OnEnable()
    {
        base.OnEnable();

        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    public override void Enter()
    {
        base.Enter();

        bossController.StartCoroutine(SecondStage());
    }
    
    IEnumerator SecondStage()
    {
        bossController.motor.SetPosition(warpPosition);

        PoolManager.Release(attackVFX, bossTransform.TransformPoint(attackVFXOffset));
        
        timer = 0;
        while (timer < waitSecondAnimationTime)
        {
            timer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        animator.CrossFade(secondAnimationName, transDuration);

        ReturnBaseState();
    }
}
