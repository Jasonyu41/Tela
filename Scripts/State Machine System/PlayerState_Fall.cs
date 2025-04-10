using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall", fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (input.isJumpDown)
        {
            if (input.isJumpDown && characterController.canAirJump)
            {
                characterController.SwitchState(typeof(PlayerState_AirJump));

                return;
            }

            characterController.SetJumpInputBufferTimer();
        }

        if (input.isDashDown && characterController.canAirDash)
        {
            characterController.SwitchState(typeof(PlayerState_AirDash));
        }

        if (groundDetector.hasGrounded)
        {
            characterController.SwitchState(typeof(PlayerState_Land));
        }
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        base.UpdateVelocity(ref currentVelocity, deltaTime);

        if (characterController.cameraInputMovement.sqrMagnitude > 0f)
        {
            Vector3 targetMovementVelocity = characterController.cameraInputMovement * characterController.airMoveSpeed;

            if (motor.GroundingStatus.FoundAnyGround)
            {
                Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp).normalized;
                targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
            }

            Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, characterController.Gravity);
            currentVelocity += velocityDiff * characterController.airAccelerationSpeed * deltaTime;
        }

        if (characterController.isUseGravity)
        {
            currentVelocity += characterController.Gravity * deltaTime;
        }
    }
}
