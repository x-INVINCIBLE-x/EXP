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
}
