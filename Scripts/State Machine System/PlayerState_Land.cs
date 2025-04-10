using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Land", fileName = "PlayerState_Land")]
public class PlayerState_Land : PlayerState
{
    public override void Enter()
    {
        base.Enter();

        characterController.canAirJump = true;
        characterController.canAirDash = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if ((characterController.hasJumpInputBuffer || input.isJumpDown) && characterController.motor.GroundingStatus.IsStableOnGround)
        {
            characterController.SwitchState(typeof(PlayerState_JumpUp));
        }

        if (input.isDashDown)
        {
            characterController.SwitchState(typeof(PlayerState_Dash));
        }

        if (IsAnimationFinished)
        {
            characterController.SwitchState(typeof(PlayerState_Default));
        }

        if (StateDuration < characterController.landStiffTime) return;

        if (input.isMoveDown)
        {
            characterController.SwitchState(typeof(PlayerState_Default));
        }
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        base.UpdateVelocity(ref currentVelocity, deltaTime);

        if (characterController.cameraInputMovement.sqrMagnitude > 0f)
        {
            Vector3 targetMovementVelocity = characterController.cameraInputMovement * characterController.landMoveSpeed;

            if (motor.GroundingStatus.FoundAnyGround)
            {
                Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp).normalized;
                targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
            }

            Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, characterController.Gravity);
            currentVelocity += velocityDiff * characterController.landAccelerationSpeed * deltaTime;
        }
        else
        {
            currentVelocity.x = 0;
            currentVelocity.z = 0;
        }

        if (!motor.GroundingStatus.IsStableOnGround && characterController.isUseGravity)
        {
            currentVelocity += characterController.Gravity * deltaTime;
        }
    }
}
