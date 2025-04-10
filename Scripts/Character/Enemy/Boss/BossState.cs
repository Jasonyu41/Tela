using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : ScriptableObject
{
    [Header("Animation Setting")]
    [SerializeField] string stateAnimationName;
    [SerializeField, Range(0f, 1f)] float transitionDuration = 0.1f;
    [SerializeField] float waitAnimationTimeToStart;

    [Header("Effects Setting")]
    [SerializeField] protected GameObject attackVFX;
    [SerializeField] protected AudioClip attackSFX;

    [Header("Misc")]
    [SerializeField] float waitReturnBaseStateTime;
    [SerializeField] bool isTrackingPlayer = false;


    protected Boss boss;
    protected BossController bossController;
    protected Transform bossTransform;
    protected Transform playerTransform;
    protected Animator animator;
    protected bool IsAnimationFinished => isStartToPlayAnimation && StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    protected float StateDuration => Time.time - stateStartTime;

    int stateHash;
    float stateStartTime;
    float animationStartTimer;
    float returnBaseStateTimer;
    bool isStartToPlayAnimation;

    WaitForFixedUpdate waitForFixedUpdate;


    protected virtual void OnEnable()
    {
        stateHash = Animator.StringToHash(stateAnimationName);
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    protected virtual void OnDisable()
    {
        
    }

    public virtual void Initialize(Boss boss, BossController bossController, Transform playerTransform, Animator animator)
    {
        this.boss = boss;
        this.bossController = bossController;
        bossTransform = boss.transform;
        this.playerTransform = playerTransform;
        this.animator = animator;
    }
    
    public virtual void Enter()
    {
        stateStartTime = Time.time;
        isStartToPlayAnimation = false;

        bossController.StartCoroutine(WaitAnimationStart());
    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {
        
    }

    public void UpdatePlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    IEnumerator WaitAnimationStart()
    {
        animationStartTimer = 0;
        while (animationStartTimer < waitAnimationTimeToStart)
        {
            animationStartTimer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        bossController.isTrackingPlayer = isTrackingPlayer;
        isStartToPlayAnimation = true;

        if (stateAnimationName != null)
        {
            animator.CrossFade(stateHash, transitionDuration);
        }
    }

    protected void ReturnBaseState()
    {
        bossController.StartCoroutine(ReturnBaseStateController());
    }

    IEnumerator ReturnBaseStateController()
    {
        returnBaseStateTimer = 0;
        while (returnBaseStateTimer < waitReturnBaseStateTime)
        {
            returnBaseStateTimer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }
        
        bossController.SwitchStateToDefault();
    }
}
