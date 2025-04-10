using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class PlayerCharacterController : MonoBehaviour, ICharacterController
{
    [Header("Move")]
    [SerializeField] float walkSpeed = 1.5f;
    [SerializeField] float runSpeed = 5.5f;
    [SerializeField] float aimWalkSpeed = 1.5f;
    [SerializeField] float aimRunSpeed = 5.5f;
    [SerializeField] public float speedMagnification = 1;
    [SerializeField] float addRotation = 120f;
    [SerializeField] float interpolationVerticalSpeed = 10;
    [SerializeField] float interpolationTurnSpeed = 5;
    [SerializeField] public float coyoteTime = 0.15f;

    [Header("Air Move")]
    [SerializeField] public float airMoveSpeed = 10f;
    [SerializeField] public float airAccelerationSpeed = 5f;
    [SerializeField] public float landMoveSpeed = 4f;
    [SerializeField] public float landAccelerationSpeed = 2f;

    [Header("Jump")]
    [SerializeField] public float jumpSpeed = 15f;
    [SerializeField] public float airJumpSpeed = 10f;
    [SerializeField] public float jumpInputBufferTime = 0.2f;
    [SerializeField] public float landStiffTime = 0.1f;
    // public bool AllowJumpingWhenSliding = false;    //是否能夠在不平整的地板跳躍
    [HideInInspector] public bool hasJumpInputBuffer = false;
    [HideInInspector] public bool canAirJump = true;

    [Header("Dash")]
    [SerializeField] float invincibleTime = 0.66f;
    [SerializeField] public float dashMagnification = 1;
    [SerializeField] public float airDashMagnification = 1;
    // [SerializeField] public float dashInputBufferTime = 0.2f;
    // [HideInInspector] public bool hasDashInputBuffer = false;
    [HideInInspector] public bool canAirDash = true;

    [Header("Animator Parameters")]
    [SerializeField] string verticalSpeedName = "Vertical Speed";
    [SerializeField] string horizontalSpeedName = "Horizontal Speed";
    [SerializeField] string turnSpeedName = "Turn Speed";
    [SerializeField] string inputVerticalName = "Input Vertical";
    [SerializeField] string inputHorizontalName = "Input Horizontal";
    [SerializeField] string isAimingName = "isAiming";
    int verticalSpeedHash;
    int horizontalSpeedHash;
    int turnSpeedHash;
    [HideInInspector] public int inputVerticalHash;
    [HideInInspector] public int inputHorizontalHash;
    int isAimingHash;

    [Header("Misc")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerState[] states;
    [SerializeField] public Vector3 Gravity = new Vector3(0, -30f, 0);
    [SerializeField] bool isHookLock = false;
    [SerializeField] GetHookLock getHookLock;
    [SerializeField] public HookLockTrigger hookLock;

#if UNITY_EDITOR
    [Header("Developer mode")]
    [SerializeField] float developerSpeed = 10f;
    [SerializeField] float upSpeed = 10f;
    [SerializeField] float downSpeed = 10f;
#endif
    [HideInInspector] public bool isUseGravity = false;


    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public System.Type beforeStateType;
    protected Dictionary<System.Type, PlayerState> stateTable;

    float moveVerticalSpeed;
    float moveHorizontalSpeed;
    float moveTurnSpeed;
    Vector3 cameraForwardProjection;
    [HideInInspector] public Vector3 cameraInputMovement;                           //相機方向輸入方向
    [HideInInspector] public Vector3 characterInputMovement;                        //角色方向輸入方向
    [HideInInspector] public Vector3 rootMotionPositionDelta;
    Quaternion rootMotionRotationDelta;

    Animator animator;
    [HideInInspector] public KinematicCharacterMotor motor;                     //PartyManager Using
    Player player;
    PlayerGroundDetector groundDetector;

    Camera mainCamera;
    Transform tf;
    Transform mainCameraTransform;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    WaitForSeconds waitForOneSeconds;
    WaitForSeconds waitForJumpInputBufferTime;
    // WaitForSeconds waitForDashInputBufferTime;
    WaitForSeconds waitForInvincibleTime;

    Coroutine moveCoroutine;


    private void Awake()
    {
        motor = GetComponent<KinematicCharacterMotor>();
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        groundDetector = GetComponentInChildren<PlayerGroundDetector>();

        motor.CharacterController = this;
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
        tf = transform;
        rootMotionPositionDelta = Vector3.zero;
        rootMotionRotationDelta = Quaternion.identity;
        verticalSpeedHash = Animator.StringToHash(verticalSpeedName);
        horizontalSpeedHash = Animator.StringToHash(horizontalSpeedName);
        turnSpeedHash = Animator.StringToHash(turnSpeedName);
        inputVerticalHash = Animator.StringToHash(inputVerticalName);
        inputHorizontalHash = Animator.StringToHash(inputHorizontalName);
        isAimingHash = Animator.StringToHash(isAimingName);

        waitForOneSeconds = new WaitForSeconds(1f);
        waitForJumpInputBufferTime = new WaitForSeconds(jumpInputBufferTime);
        // waitForDashInputBufferTime = new WaitForSeconds(dashInputBufferTime);
        waitForInvincibleTime = new WaitForSeconds(invincibleTime);

        stateTable = new Dictionary<System.Type, PlayerState>(states.Length);
        foreach (PlayerState state in states)
        {
            state.Initialize(animator, this, motor, playerInput, groundDetector);
            stateTable.Add(state.GetType(), state);
        }
        
        isUseGravity = true;
    }

    private void OnEnable()
    {
        playerInput.onMove += Move;
        playerInput.onStopMove += StopMove;
        playerInput.onSwitchCharacter += SwitchCharacter;
        playerInput.onLockPerspective += ChangeAim;
    }

    private void OnDisable()
    {
        playerInput.onMove -= Move;
        playerInput.onStopMove -= StopMove;
        playerInput.onSwitchCharacter -= SwitchCharacter;
        playerInput.onLockPerspective -= ChangeAim;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveVerticalSpeed = 0;
        moveHorizontalSpeed = 0;
        moveTurnSpeed = 0;
        animator.SetFloat(verticalSpeedHash, 0);
        animator.SetFloat(horizontalSpeedHash, 0);
        animator.SetFloat(turnSpeedHash, 0);
    }

    private void Start()
    {
        // if (!StartMovie.isFirstInGame)
        // {
            playerInput.EnableGameplayInputs();
        // }

        beforeStateType = typeof(PlayerState_Default);
        SwitchOn(stateTable[typeof(PlayerState_Default)]);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            isUseGravity = !isUseGravity;
        }

        if (!isUseGravity)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                motor.SetPosition(transform.position + Vector3.up * upSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                motor.SetPosition(transform.position + Vector3.up * -1f * downSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.W))
            {
                motor.SetPosition(transform.position + cameraForwardProjection * developerSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                motor.SetPosition(transform.position + cameraForwardProjection * -1 * developerSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                motor.SetPosition(transform.position + mainCameraTransform.right * -1 * developerSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                motor.SetPosition(transform.position + mainCameraTransform.right * developerSpeed * Time.deltaTime);
            }
        }
#endif

        currentState.LogicUpdate();

        if (isHookLock && Input.GetKeyDown(KeyCode.F))
        {
            hookLock = getHookLock.DetectionHookLock(transform.TransformPoint(new Vector3(0, 1, 0)));
            if (hookLock != null) SwitchState(typeof(PlayerState_HookLock));
        }
    }

    private void FixedUpdate()
    {
        currentState.PhysicUpdate();
    }

    private void OnAnimatorMove()
    {
        rootMotionPositionDelta += animator.deltaPosition;
        rootMotionRotationDelta = animator.deltaRotation * rootMotionRotationDelta;
    }

#region State Machine
    protected void SwitchOn(PlayerState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    public void SwitchState(PlayerState newState)
    {
        currentState.Exit();
        beforeStateType = currentState.GetType();
        SwitchOn(newState);
    }

    public void SwitchState(System.Type newStateType)
    {
        SwitchState(stateTable[newStateType]);
    }
#endregion

#region Kinematic Character Controller
    public void BeforeCharacterUpdate(float deltaTime){}
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (/*!playerInput.isAiming && */currentState != stateTable[typeof(PlayerState_Dash)] && currentState != stateTable[typeof(PlayerState_AirDash)])
        {
            currentRotation = rootMotionRotationDelta * currentRotation;
            currentRotation = Quaternion.Euler(0, moveTurnSpeed * addRotation * deltaTime, 0f) * currentRotation;
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        currentState?.UpdateVelocity(ref currentVelocity, deltaTime);
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        rootMotionPositionDelta = Vector3.zero;
        rootMotionRotationDelta = Quaternion.identity;
    }

    public bool IsColliderValidForCollisions(Collider coll){ return true; }
    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}
    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}
    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport){}
    public void PostGroundingUpdate(float deltaTime){}
    public void OnDiscreteCollisionDetected(Collider hitCollider){}
#endregion

#region Player Input
#region Move
    private void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(moveInput));
        StopCoroutine(nameof(DecelerationCoroutine));
    }

    private void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(Vector2.zero));
        StartCoroutine(nameof(DecelerationCoroutine));
    }
    
    IEnumerator MoveCoroutine(Vector2 moveInput)
    {
        while(true)
        {
            cameraForwardProjection = mainCameraTransform.forward;
            cameraForwardProjection.y = 0;
            cameraForwardProjection = cameraForwardProjection.normalized;

            cameraInputMovement = cameraForwardProjection * moveInput.y + mainCameraTransform.right * moveInput.x;
            characterInputMovement = tf.InverseTransformVector(cameraInputMovement);

            float targetMoveVerticalSpeed = 0f;
            float targetMoveHorizontalSpeed = 0f;

            if (moveInput != Vector2.zero)
            {
                targetMoveVerticalSpeed = runSpeed * characterInputMovement.z;
                targetMoveHorizontalSpeed = runSpeed * characterInputMovement.x;
            }

            moveVerticalSpeed = Mathf.Lerp(moveVerticalSpeed, moveInput == Vector2.zero? 0f : (runSpeed - walkSpeed) * characterInputMovement.z + Mathf.Sign(characterInputMovement.z) * walkSpeed, 1f - Mathf.Exp(-interpolationVerticalSpeed * Time.fixedDeltaTime * (playerInput.isMoveDown ? 1 : 2)));
            moveHorizontalSpeed = Mathf.Lerp(moveHorizontalSpeed, moveInput == Vector2.zero? 0f : (runSpeed - walkSpeed) * characterInputMovement.x + Mathf.Sign(characterInputMovement.x) * walkSpeed, 1f - Mathf.Exp(-interpolationVerticalSpeed * Time.fixedDeltaTime * (playerInput.isMoveDown ? 1 : 2)));
            // moveVerticalSpeed = Mathf.Lerp(moveVerticalSpeed, targetMoveVerticalSpeed, 1f - Mathf.Exp(-interpolationVerticalSpeed * Time.fixedDeltaTime * (playerInput.isMoveDown ? 1 : 2)));
            // moveHorizontalSpeed = Mathf.Lerp(moveHorizontalSpeed, targetMoveHorizontalSpeed, 1f - Mathf.Exp(-interpolationVerticalSpeed * Time.fixedDeltaTime * (playerInput.isMoveDown ? 1 : 2)));
            moveTurnSpeed = Mathf.Lerp(moveTurnSpeed, Mathf.Atan2(characterInputMovement.x, characterInputMovement.z), 1f - Mathf.Exp(-interpolationTurnSpeed * Time.fixedDeltaTime));
            animator.SetFloat(verticalSpeedHash, moveVerticalSpeed);
            animator.SetFloat(horizontalSpeedHash, moveHorizontalSpeed);
            animator.SetFloat(turnSpeedHash, moveTurnSpeed);

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitForOneSeconds;

        StopCoroutine(moveCoroutine);
        moveVerticalSpeed = 0;
        moveTurnSpeed = 0;
        animator.SetFloat(verticalSpeedHash, 0);
        animator.SetFloat(horizontalSpeedHash, 0);
        animator.SetFloat(turnSpeedHash, 0);
    }
#endregion    

#region Jump
    public void SetJumpInputBufferTimer()
    {
        StopCoroutine(nameof(JumpInputBufferCoroutine));
        StartCoroutine(nameof(JumpInputBufferCoroutine));
    }

    IEnumerator JumpInputBufferCoroutine()
    {
        hasJumpInputBuffer = true;

        yield return waitForJumpInputBufferTime;

        hasJumpInputBuffer = false;
    }
#endregion

#region Dash
    // public void SetDashInputBufferTimer()
    // {
    //     StopCoroutine(nameof(DashInputBufferCoroutine));
    //     StartCoroutine(nameof(DashInputBufferCoroutine));
    // }

    // IEnumerator DashInputBufferCoroutine()
    // {
    //     hasDashInputBuffer = true;

    //     yield return waitForDashInputBufferTime;

    //     hasDashInputBuffer = false;
    // }
#endregion

#region SwitchCharacter
    private void SwitchCharacter(float value)
    {
        PartyManager.SwitchPartyMember((int)((value + 3f) / 2f));
    }
#endregion
#region Aim
    private void ChangeAim()
    {
        // 偵測目標是否在範圍內 否則 isAiming = false;
        // animator.SetBool(isAimingHash, playerInput.isAiming);
        if (GameManager.Instance.canLookAtBoss)
        {
            (player.cinemachineFreeLook.Priority, player.cinLookAt.Priority) = (player.cinLookAt.Priority, player.cinemachineFreeLook.Priority);
            GameManager.Instance.isLookAtEnemy = !GameManager.Instance.isLookAtEnemy;
            playerInput.isAiming = !playerInput.isAiming;
        }
    }
#endregion
#endregion

#region Animation Events
    public void SwitchCollision()
    {
        StartCoroutine(InvincibleTimer());
    }
    
    IEnumerator InvincibleTimer()
    {
        motor.Capsule.enabled = false;

        yield return waitForInvincibleTime;

        motor.Capsule.enabled = true;
    }
#endregion

#if UNITY_EDITOR
    void OnGUI()
    {
        Rect rect = new Rect(200, 200, 200, 200);

        string message = "";
        message +=  "CurrentState : " + currentState.name;
        message += "\n" + "IsMoveDown: " + playerInput.isMoveDown;
        message += "\n" + "HasJumpInputBuffer: " + hasJumpInputBuffer;
        message += "\n" + "IsStableOnGround: " + motor.GroundingStatus.IsStableOnGround;
        message += "\n" + "characterInputMovement: " + characterInputMovement;
        message += "\n" + "characterInputMovement.magnitude: " + characterInputMovement.magnitude;
        message = "";

        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;

        GUI.Label(rect, message, style);
    }
#endif
}
