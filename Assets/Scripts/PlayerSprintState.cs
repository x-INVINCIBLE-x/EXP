using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerState
{
    private float timer = 0;
    public PlayerSprintState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        MovementTimer();

        if (timer > 0.12f || !player.inputManager.isSprinting)
        {
            Debug.Log("Timer: " + timer);
            ChangeToLocomotion();
        }

        Vector3 movement = CalculateMovement();
        Move(movement, player.runSpeed);
        FreeLookDirection(movement);
    }
    public override void Exit()
    {
        base.Exit();
    }

    private void MovementTimer()
    {
        if (player.inputManager.Movement == Vector2.zero)
            timer += Time.deltaTime;
        else
            timer = 0;
    }

}
