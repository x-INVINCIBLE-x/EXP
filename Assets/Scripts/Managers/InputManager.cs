using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, PlayerControls.IPlayerActions
{
    private PlayerControls controls;
    public Vector2 Movement { get; private set; }

    public event Action TargetEvent;
    public event Action SprintEvent;
    public event Action DodgeEvent;
    public event Action LightAttackEvent;
    public event Action HeavyAttackEvent;
    public event Action ChargeAttackEvent;
    public event Action WeaponSwitchEvent;
    public event Action FableArtsEvent;
    
    public bool isSprinting = false;
    public bool isHolding = false;
    
    private void Start()
    {
        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }


    public void OnTarget(InputAction.CallbackContext context)
    {
        TargetEvent?.Invoke();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.performed)
            SprintEvent?.Invoke();

        if(context.canceled)
            isSprinting = false;
        else
            isSprinting = true;
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
            DodgeEvent?.Invoke();
        
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        LightAttackEvent?.Invoke();
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
            HeavyAttackEvent?.Invoke();
    }

    public void OnChargeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChargeAttackEvent?.Invoke();
    }

    public void OnWeaponSwitch(InputAction.CallbackContext context)
    {
        WeaponSwitchEvent?.Invoke();
    }

    public void OnFableArts(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            FableArtsEvent?.Invoke();
            isHolding = true;
        }

        if(context.canceled)
            isHolding = false;
    }
}
