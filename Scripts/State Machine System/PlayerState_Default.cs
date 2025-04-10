using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Default", fileName = "PlayerState_Default")]
public class PlayerState_Default : PlayerState
{
    [SerializeField, Range(0f, 1f)]  float landToDefaultTransitionDuration;
    [SerializeField, Range(0f, 1f)]  float landToRunTransitionDuration;
    [SerializeField, Range(0f, 1f)]  float dashToDefaultTransitionDuration;
    [SerializeField] bool isEnableDashToDefault = true;
    [SerializeField, Range(0f, 1f)]  float otherTransitionDuration;

    public override void Enter()
    {
        if (characterController.beforeStateType == typeof(PlayerState_Land))
        {
            if (input.isMoveDown)
            {
                PlayAnimation(landToRunTransitionDuration);
            }
            else
            {
                PlayAnimation(landToDefaultTransitionDuration);
            }
        }else if (characterController.beforeStateType == typeof(PlayerState_Dash))
        {
            //男主Animator自動插值回Default
            if (!isEnableDashToDefault) return ;
            PlayAnimation(dashToDefaultTransitionDuration);
        }
        else
        {
            PlayAnimation(otherTransitionDuration);
        }

        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (input.isJumpDown && (characterController.motor.GroundingStatus.IsStableOnGround || groundDetector.hasGrounded))
        {
            characterController.SwitchState(typeof(PlayerState_JumpUp));
        }

        if (input.isDashDown)
        {
            characterController.SwitchState(typeof(PlayerState_Dash));
        }

        if (!motor.GroundingStatus.FoundAnyGround && !groundDetector.hasGrounded)
        {
            characterController.SwitchState(typeof(PlayerState_CoyoteTime));
        }
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        base.UpdateVelocity(ref currentVelocity, deltaTime);

        if (motor.GroundingStatus.IsStableOnGround)
        {
            if (deltaTime > 0)
            {
                currentVelocity = characterController.rootMotionPositionDelta / deltaTime * ((characterController.speedMagnification - 1) * characterController.characterInputMovement.magnitude + 1);
                currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;
            }
            else
            {
                currentVelocity = Vector3.zero;
            }
        }
        else
        {
            if (characterController.isUseGravity)
            {
                currentVelocity += characterController.Gravity * deltaTime;
            }
        }
    }
}
