using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/HookLock", fileName = "PlayerState_HookLock")]
public class PlayerState_HookLock : PlayerState
{
    [SerializeField] float chargeTime;
    [SerializeField] float moveTime;

    Vector3 targetPosition;
    Vector3 distance;
    bool isHookLock;
    float timer;
    float timer2;

    public override void Enter()
    {
        base.Enter();

        characterController.hookLock.ChangeImage(true);
        targetPosition = characterController.hookLock.highestPoint.position;
        isHookLock = false;
        timer2 = 0;
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        if (!isHookLock && timer2 >= chargeTime)
        {
            motor.ForceUnground(0.1f);
            distance = targetPosition - characterController.motor.TransientPosition;
            motor.BaseVelocity = distance / moveTime;
            isHookLock = true;
            timer = 0;
        }
        if (timer2 < chargeTime) timer2 += Time.fixedDeltaTime;
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        base.UpdateVelocity(ref currentVelocity, deltaTime);

        if (isHookLock)
        {
            timer += deltaTime;
            if (timer > moveTime)
            {
                characterController.hookLock.ChangeImage(false);
                characterController.SwitchState(typeof(PlayerState_Default));
            }
        }
    }
}
