using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerState
{
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

        if (player.inputManager.Movement == Vector2.zero || !player.inputManager.isSprinting)
            ChangeToLocomotion();

        Vector3 movement =  CalculateMovement();
        Move(movement, player.runSpeed);
        FreeLookDirection(movement);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
