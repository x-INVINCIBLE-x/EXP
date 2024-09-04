using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, PlayerControls.IPlayerActions
{
    public static InputManager Instance;
    private PlayerControls controls;
    public Vector2 Movement { get; private set; }

    public event Action TargetEvent;
    public event Action SprintEvent;
    public event Action DodgeEvent;
    public event Action LightAttackEvent;
    public event Action HeavyAttackEvent;
    public event Action ChargeAttackEvent;
    public event Action BlockEvent;
    public event Action WeaponSwitchEvent;
    public event Action FableArtsEvent;
    public event Action BackEvent;
    public event Action UpdateUpperBeltEvent;
    public event Action UpdateLowerBeltEvent;
    public event Action UseEvent;
    public event Action Interact;
    public event Action<int> On1Event;
    public event Action<int> On2Event;
    public event Action<int> On3Event;
    public event Action<int> On4Event;

    public bool isSprinting = false;
    public bool isHolding = false;
    public bool isBlocking = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

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
        if(context.performed)
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
            isHolding = true;

        if(context.performed)
            FableArtsEvent?.Invoke();

        if(context.canceled)
            isHolding = false;
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isBlocking = false;
            return;
        }
        
        isBlocking = true;
        BlockEvent?.Invoke();
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.performed)
            BackEvent?.Invoke();
    }

    public void OnUpdateUpperBelt(InputAction.CallbackContext context)
    {
        if (context.performed)
            UpdateUpperBeltEvent?.Invoke();
    }

    public void OnUpdateLowerBelt(InputAction.CallbackContext context)
    {
        if (context.performed)
            UpdateLowerBeltEvent?.Invoke();
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
           UseEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            Interact?.Invoke();
    }

    public void On_1(InputAction.CallbackContext context)
    {
        if (context.performed)
            On1Event?.Invoke(0);
    }

    public void On_2(InputAction.CallbackContext context)
    {
        if (context.performed)
            On2Event?.Invoke(1);
    }

    public void On_3(InputAction.CallbackContext context)
    {
        if (context.performed)
            On3Event?.Invoke(2);
    }

    public void On_4(InputAction.CallbackContext context)
    {
        if (context.performed)
            On4Event?.Invoke(3);
    }

    public void ClearInteractEvent()
    {
        Interact = null;
    }
}
