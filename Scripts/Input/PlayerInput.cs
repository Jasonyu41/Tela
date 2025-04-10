using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, PlayerInputAction.IGamePlayActions, PlayerInputAction.IUIActions
{
    public event UnityAction<Vector2> onMove = delegate {};
    public event UnityAction onStopMove = delegate {};
    public event UnityAction<Vector2> onViewMove = delegate {};
    public event UnityAction onStopViewMove = delegate {};
    public event UnityAction onJump = delegate {};
    public event UnityAction onStopJump = delegate {};
    public event UnityAction onDash = delegate {};
    public event UnityAction onStopDash = delegate {};
    public event UnityAction<float> onSwitchCharacter = delegate {};
    public event UnityAction onLightAttack = delegate {};
    public event UnityAction onHeavyAttack = delegate {};
    public event UnityAction onLockPerspective = delegate {};    
    public event UnityAction onSkill = delegate {};
    public event UnityAction onStopSkill = delegate {};
    public event UnityAction onFrontSkill = delegate {};
    public event UnityAction onStopFrontSkill = delegate {};
    public event UnityAction onBackSkill = delegate {};
    public event UnityAction onStopBackSkill = delegate {};
    public event UnityAction onLeftSkill = delegate {};
    public event UnityAction onStopLeftSkill = delegate {};
    public event UnityAction onRightSkill = delegate {};
    public event UnityAction onStopRightSkill = delegate {};
    public event UnityAction onPause = delegate {};
    public event UnityAction onInteracive = delegate {};    
    public event UnityAction onUnPause = delegate {};
    public event UnityAction onCancel = delegate {};
    public event UnityAction onUIMoveLeft = delegate {};
    public event UnityAction onUIMoveRight = delegate {};

    public bool isMoveDown = false;
    public bool isViewMoveDown = false;
    public bool isJumpDown = false;
    public bool isJumpThisFrame => inputActions.GamePlay.Jump.WasPressedThisFrame();
    public bool isDashDown = false;
    public bool isDashThisFrame => inputActions.GamePlay.Dash.WasPressedThisFrame();
    public bool isAiming = false;
    public bool isSkillDown = false;
    public bool isFrontSkillDown = false;
    public bool isBackSkillDown = false;
    public bool isLeftSkillDown = false;
    public bool isRightSkillDown = false;
    
    PlayerInputAction inputActions;

    void OnEnable()
    {
        inputActions = new PlayerInputAction();

        inputActions.GamePlay.SetCallbacks(this);
        inputActions.UI.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInputs();
    }
    
    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInputs() => inputActions.Disable();

    public void EnableGameplayInputs() => SwitchActionMap(inputActions.GamePlay, false);

    public void EnableUIInput() => SwitchActionMap(inputActions.UI, true);

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMoveDown = true;
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            isMoveDown = false;
            onStopMove.Invoke();
        }
    }

    public void OnViewMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isViewMoveDown = true;
            onViewMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            isViewMoveDown = false;
            onStopViewMove.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumpDown = true;
            onJump.Invoke();
            
            GameManager.Instance.StartCoroutine(JumpCoroutine());
        }

        if (context.canceled)
        {
            isJumpDown = false;
            onStopJump.Invoke();
        }
    }

    IEnumerator JumpCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return null;
        }
        isJumpDown = false;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDashDown = true;
            onDash.Invoke();

            GameManager.Instance.StartCoroutine(DashCoroutine());
        }

        if (context.canceled)
        {
            isDashDown = false;
            onStopDash.Invoke();
        }
    }

    IEnumerator DashCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return null;
        }
        isDashDown = false;
    }

    public void OnSwitchCharacter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onSwitchCharacter.Invoke(context.ReadValue<float>());
        }
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLightAttack.Invoke();
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onHeavyAttack.Invoke();
        }
    }

    public void OnLockPerspective(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLockPerspective.Invoke();

            // isAiming = !isAiming;
        }
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSkillDown = true;
            onSkill.Invoke();
        }

        if (context.canceled)
        {
            isSkillDown = false;
            onStopSkill.Invoke();
        }
    }

    public void OnFrontSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFrontSkillDown = true;
            onFrontSkill.Invoke();
        }

        if (context.canceled)
        {
            isFrontSkillDown = false;
            onStopFrontSkill.Invoke();
        }
    }

    public void OnBackSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isBackSkillDown = true;
            onBackSkill.Invoke();
        }

        if (context.canceled)
        {
            isBackSkillDown = false;
            onStopBackSkill.Invoke();
        }
    }

    public void OnLeftSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isLeftSkillDown = true;
            onLeftSkill.Invoke();
        }

        if (context.canceled)
        {
            isLeftSkillDown = false;
            onStopLeftSkill.Invoke();
        }
    }

    public void OnRightSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRightSkillDown = true;
            onRightSkill.Invoke();
        }

        if (context.canceled)
        {
            isRightSkillDown = false;
            onStopRightSkill.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnInteracive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onInteracive.Invoke();
        }
    }

    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }

    public void OnMoveSelection(InputAction.CallbackContext context)
    {
        // throw new System.NotImplementedException();
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        // throw new System.NotImplementedException();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onCancel.Invoke();
        }
    }

    public void OnUIMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUIMoveLeft.Invoke();
        }
    }

    public void OnUIMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUIMoveRight.Invoke();
        }
    }
}
