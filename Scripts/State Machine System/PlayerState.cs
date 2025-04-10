using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class PlayerState : ScriptableObject, ICharacterController
{
    [SerializeField] bool isPlayAnimation = true;
    [SerializeField] string stateName;
    [SerializeField, Range(0f, 1f)] float transitionDuration = 0.1f;

    float stateStartTime;
    int stateHash;

    protected Animator animator;
    protected PlayerCharacterController characterController;
    protected KinematicCharacterMotor motor;
    protected PlayerInput input;
    protected PlayerGroundDetector groundDetector;

    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;
    protected float StateDuration => Time.time - stateStartTime;


    void OnEnable()
    {
        stateHash = Animator.StringToHash(stateName);
    }

    public void Initialize(Animator animator, PlayerCharacterController characterController, KinematicCharacterMotor motor, PlayerInput input, PlayerGroundDetector groundDetector)
    {
        this.animator = animator;
        this.characterController = characterController;
        this.motor = motor;
        this.input = input;
        this.groundDetector = groundDetector;
    }

    public virtual void Enter()
    {
        if (isPlayAnimation && !string.IsNullOrEmpty(stateName))
        {
            PlayAnimation(transitionDuration);
        }
        stateStartTime = Time.time;
    }

    public virtual void Exit(){}
    public virtual void LogicUpdate(){}
    public virtual void PhysicUpdate(){}

    public virtual void BeforeCharacterUpdate(float deltaTime){}
    public virtual void UpdateRotation(ref Quaternion currentRotation, float deltaTime){}
    public virtual void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime){}
    public virtual void AfterCharacterUpdate(float deltaTime){}
    public virtual bool IsColliderValidForCollisions(Collider coll){ return true; }
    public virtual void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}
    public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}
    public virtual void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport){}
    public virtual void PostGroundingUpdate(float deltaTime){}
    public virtual void OnDiscreteCollisionDetected(Collider hitCollider){}

    protected void PlayAnimation(float _transitionDuration)
    {
        animator.CrossFade(stateHash, _transitionDuration);
    }
}
