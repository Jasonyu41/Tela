using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System.Linq;

public class BossController : MonoBehaviour, ICharacterController
{
    [Header("Boss State")]
    [SerializeField] Boss_Default bossDefault;
    [SerializeField] Boss_SecondStage bossSecondStage;
    [SerializeField] BossState[] bossStates;
    Dictionary<string, BossState> stateModeTable;
    [HideInInspector] public BossState currentStete;
    [HideInInspector] public BossState nextState;

    [Header("Move")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float slowMoveSpeed = 2.5f;
    [SerializeField] float stableMovementSharpness = 15f;
    [SerializeField] public float stableTurnSharpness = 8f;
    [SerializeField] public Vector3[] moveFrontDirection;
    [SerializeField] public Vector3[] moveBackDirection;
    [SerializeField] float maxHight = 0.5f;
    [SerializeField] float minHight = 0.5f;
    [SerializeField] Transform[] moveRange;
    float originalHight;
    Vector2[] moveRangeVector2;

    [Header("Attack Distance")]
    [SerializeField] float innerCircleRadius = 5f;
    [SerializeField] float outerCircleRadius = 15f;

    [Header("Misc")]
    [SerializeField] float[] outLineBackTime = {1.5f, 2.5f};
    [SerializeField] VoidEventChannel switchPartyMemberEventChannel;


    [HideInInspector] public KinematicCharacterMotor motor;
    [HideInInspector] public BossAndPlayerDistance bossAndPlayerDistance;
    protected Boss boss;
    protected Animator animator;

    protected Transform playerTransform;
    Vector3 moveInput;
    Vector3 characterInputMovement;
    Vector3 targetVelocity;
    Vector3 targetPosition;
    Vector2 targetVector2;
    float targetAngle;

    [HideInInspector] public bool isTrackingPlayer = false;
    [HideInInspector] public bool isOutLine = false;


    protected virtual void Awake()
    {
        motor = GetComponent<KinematicCharacterMotor>();
        boss = GetComponent<Boss>();
        animator = GetComponent<Animator>();

        motor.CharacterController = this;
        playerTransform = GameObject.Find("Player").transform;      // 更改 Find 仇恨 LIST

        stateModeTable = new Dictionary<string, BossState>(bossStates.Length + 1);

        bossDefault.Initialize(boss, this, playerTransform, animator);
        bossSecondStage.Initialize(boss, this, playerTransform, animator);
        foreach (BossState bossAttackMode in bossStates)
        {
            bossAttackMode.Initialize(boss, this, playerTransform, animator);
            stateModeTable.Add(bossAttackMode.name, bossAttackMode);
        }

        moveRangeVector2 = new Vector2[moveRange.Length];
        for (int i = 0; i < moveRange.Length; i++)
        {
            moveRangeVector2[i] = new Vector2(moveRange[i].position.x, moveRange[i].position.z);
        }
    }
    
    private void OnEnable()
    {
        originalHight = transform.position.y;
        switchPartyMemberEventChannel.AddListener(UpdatePlayerTransforme);
    }

    private void OnDisable()
    {
        switchPartyMemberEventChannel.RemoveListener(UpdatePlayerTransforme);
    }

    private void Update()
    {
        if (Time.frameCount % 30 == 0)
        {
            UpdateBossAndPlayerDistance();
        }
        
        currentStete?.Update();

#if UNITY_EDITOR
        for (int i = 0; i < moveRange.Length; i++)
        {
            moveRangeVector2[i] = new Vector2(moveRange[i].position.x, moveRange[i].position.z);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchStateToSecondStage();
        }
#endif
    }
    
    private void FixedUpdate()
    {
        currentStete?.FixedUpdate();
    }

    public void EnterBattle()
    {
        UpdatePlayerTransforme();
        SwitchOn(bossDefault);
    }

    public virtual void SetInputs(Vector3 moveInput)
    {
        this.moveInput = moveInput;
    }

    void UpdateBossAndPlayerDistance()
    {
        bossAndPlayerDistance = BossMovementRestrictionsManager.CurrentDistance(motor.TransientPosition, playerTransform.position, innerCircleRadius, outerCircleRadius);
    }

    bool BossMoveRange(Vector2 point) => BossMovementRestrictionsManager.ContainsPoint(moveRangeVector2, point);

    IEnumerator OutLine()
    {
        isOutLine = true;
        
        yield return new WaitForSeconds(Random.Range(outLineBackTime[0], outLineBackTime[1]));

        isOutLine = false;
    }

#region KinematicCharacterController
    public void BeforeCharacterUpdate(float deltaTime){}
    
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (isTrackingPlayer)
        {
            Vector3 dir = playerTransform.position - motor.TransientPosition;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            //修正角度防止超過180
            if (Mathf.Abs(angle - targetAngle) > 180)
            {
                targetAngle += Mathf.Sign(targetAngle) * -360;
            }

            targetAngle = Mathf.Lerp(targetAngle, angle, 1 - Mathf.Exp(-stableTurnSharpness * deltaTime));
            currentRotation = Quaternion.AngleAxis(targetAngle, motor.CharacterUp);
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        characterInputMovement = Quaternion.LookRotation(motor.CharacterForward, motor.CharacterUp) * moveInput;
        targetVelocity = characterInputMovement * (bossAndPlayerDistance == BossAndPlayerDistance.Medium ? slowMoveSpeed : moveSpeed);
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1 - Mathf.Exp(-stableMovementSharpness * deltaTime));
        
        if (motor.TransientPosition.y > originalHight + maxHight || motor.TransientPosition.y < originalHight - minHight)
        {
            currentVelocity.y = Mathf.Lerp(currentVelocity.y, originalHight, 0.1f);
        }

        targetPosition = motor.TransientPosition + currentVelocity;
        targetVector2.x = targetPosition.x;
        targetVector2.y = targetPosition.z;
        if (!BossMoveRange(targetVector2))
        {
            currentVelocity = Vector3.zero;
            moveInput = moveBackDirection[Random.Range(0, moveBackDirection.Length)];
            StopCoroutine(OutLine());
            StartCoroutine(OutLine());
        }
    }

    public void AfterCharacterUpdate(float deltaTime){}

    public bool IsColliderValidForCollisions(Collider coll){ return true; }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport){}

    public void PostGroundingUpdate(float deltaTime){}

    public void OnDiscreteCollisionDetected(Collider hitCollider){}
#endregion

#region State Machine
    private void SwitchOn(BossState newState)
    {
        currentStete = newState;
        currentStete.Enter();
    }

    private void SwitchOn(string newState)
    {
        SwitchOn(stateModeTable[newState]);
    }

    public void SwitchState(BossState newState)
    {
        currentStete.Exit();
        SwitchOn(newState);
    }
    
    public void SwitchState(string newState)
    {
        SwitchState(stateModeTable[newState]);
    }

    public void SwitchStateToRND()
    {
        SwitchState(stateModeTable.ElementAt(Random.Range(0, stateModeTable.Count)).Value);
    }

    public void SwitchStateToRND(int minInclusive, int maxExclusive)
    {
        SwitchState(stateModeTable.ElementAt(Mathf.Clamp(Random.Range(minInclusive, maxExclusive), 0, stateModeTable.Count - 1)).Value);
    }
    
    public virtual void SwitchStateToDefault()
    {
        SwitchState(bossDefault);
    }

    public void SwitchStateToSecondStage()
    {
        SwitchState(bossSecondStage);
    }
#endregion

    protected virtual void UpdatePlayerTransforme()
    {
        playerTransform = PartyManager.party[0].transform;
        bossDefault.UpdatePlayerTransform(playerTransform);
        bossSecondStage.UpdatePlayerTransform(playerTransform);
        foreach (BossState bossAttackMode in bossStates)
        {
            bossAttackMode.UpdatePlayerTransform(playerTransform);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerCircleRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outerCircleRadius);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < moveRange.Length - 1; i++)
        {
            Gizmos.DrawLine(moveRange[i].position, moveRange[i + 1].position);
        }
        Gizmos.DrawLine(moveRange[moveRange.Length - 1].position, moveRange[0].position);
    }
#endif
}
