using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/CoyoteTime", fileName = "PlayerState_CoyoteTime")]
public class PlayerState_CoyoteTime : PlayerState
{
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (input.isJumpDown && characterController.motor.GroundingStatus.IsStableOnGround)
        {
            characterController.SwitchState(typeof(PlayerState_JumpUp));
        }

        if (input.isDashDown)
        {
            characterController.SwitchState(typeof(PlayerState_Dash));
        }

        if (StateDuration > characterController.coyoteTime || !input.isMoveDown)
        {
            if (!motor.GroundingStatus.FoundAnyGround && !groundDetector.hasGrounded)
            {
                characterController.SwitchState(typeof(PlayerState_Fall));
            }
            else
            {
                characterController.SwitchState(typeof(PlayerState_Default));
            }
        }
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        base.UpdateVelocity(ref currentVelocity, deltaTime);

        if (motor.GroundingStatus.IsStableOnGround)
        {
            if (deltaTime > 0)
            {
                currentVelocity = characterController.rootMotionPositionDelta / deltaTime * characterController.speedMagnification;
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
