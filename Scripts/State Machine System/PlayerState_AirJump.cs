using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/AirJump", fileName = "PlayerState_AirJump")]
public class PlayerState_AirJump : PlayerState
{
    public override void Enter()
    {
        base.Enter();

        characterController.canAirJump = false;

        Vector3 jumpDirection = motor.CharacterUp;
        if (motor.GroundingStatus.FoundAnyGround && !motor.GroundingStatus.IsStableOnGround)
        {
            jumpDirection = motor.GroundingStatus.GroundNormal;
        }
        motor.ForceUnground(0.1f);
        motor.BaseVelocity += (jumpDirection * characterController.airJumpSpeed) - Vector3.Project(motor.BaseVelocity, motor.CharacterUp);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (motor.BaseVelocity.y < 0)
        {
            characterController.SwitchState(typeof(PlayerState_Fall));
        }

        if (input.isDashDown && characterController.canAirDash)
        {
            characterController.SwitchState(typeof(PlayerState_AirDash));
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
