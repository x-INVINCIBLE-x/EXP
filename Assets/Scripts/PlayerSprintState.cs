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
        player.inputManager.DodgeEvent += OnDodge;
    }

    public override void Update()
    {
        base.Update();
        MovementTimer();

        if (timer > 0.12f || (!player.inputManager.isSprinting && timer > 0.2f))
        {
            ChangeToLocomotion();
        }

        Vector3 movement = CalculateMovement();
        Move(movement, player.runSpeed);
        FreeLookDirection(movement);
    }
    public override void Exit()
    {
        base.Exit();
        player.inputManager.DodgeEvent -= OnDodge;
    }

    private void MovementTimer()
    {
        if (player.inputManager.Movement == Vector2.zero || !player.inputManager.isSprinting)
            timer += Time.deltaTime;
        else
            timer = 0;
    }

    private void OnDodge()
    {
        Debug.Log("jump");
        stateMachine.ChangeState(player.JumpState);
    }
}
