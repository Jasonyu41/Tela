using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Boss_1/Break", fileName = "Boss_1_Break")]
public class Boss_1_Break : BossState
{
    [Header("State Setting")]
    [SerializeField] float breakTime;
    [SerializeField] string secondAnimationName;
    [SerializeField] float transDuration = 0.1f;
    [SerializeField] VoidEventChannel breakFinishedEventChannel;
    float breakTimer;
    WaitForFixedUpdate waitForFixedUpdate;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    public override void Enter()
    {
        base.Enter();

        bossController.StartCoroutine(Break());
    }

    IEnumerator Break()
    {
        breakTimer = 0;
        while (breakTimer < breakTime)
        {
            breakTimer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        breakFinishedEventChannel.Broadcast();
        animator.CrossFade(secondAnimationName, transDuration);

        ReturnBaseState();
    }
}
