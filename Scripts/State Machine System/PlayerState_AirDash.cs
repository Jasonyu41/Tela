using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/AirDash", fileName = "PlayerState_AirDash")]
public class PlayerState_AirDash : PlayerState
{
    Vector3 characterInputMovementNormalized;
    public override void Enter()
    {
        base.Enter();

        characterController.canAirDash = false;

        characterInputMovementNormalized = characterController.characterInputMovement.normalized;
        if (characterInputMovementNormalized.x == 0 && characterInputMovementNormalized.z == 0)
        {
            characterInputMovementNormalized.z = -1;
        }
        animator.SetFloat(characterController.inputVerticalHash, characterInputMovementNormalized.z * 1.1f);
        animator.SetFloat(characterController.inputHorizontalHash, characterInputMovementNormalized.x * 1.1f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (input.isJumpDown && characterController.canAirJump)
        {
            characterController.SwitchState(typeof(PlayerState_AirJump));
        }

        if (IsAnimationFinished)
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

        currentVelocity = characterController.rootMotionPositionDelta / deltaTime * characterController.airDashMagnification;
    }
}
