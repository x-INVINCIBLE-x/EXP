using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float stateTimer;
    Vector3 momentum = Vector3.zero;
    public PlayerJumpState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.5f;

        player.forceReciever.Jump(player.jumpForce);
        momentum = player.characterController.velocity;
        momentum.y = 0; 
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        Move(momentum, player.jumpLength/player.jumpDuration);
        if (player.characterController.isGrounded && stateTimer <= 0)
            stateMachine.ChangeState(new PlayerFallState(stateMachine, player, "FallRoll"));
    }

    public override void Exit()
    {
        base.Exit();
    }


}
