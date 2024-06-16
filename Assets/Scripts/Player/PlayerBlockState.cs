using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerState
{
    public PlayerBlockState(PlayerStateMachine stateMachine, Player player, string animName) : base(stateMachine, player, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StopMovement();
        player.anim.CrossFadeInFixedTime(animName, 0.05f, 0);
    }

    public override void Update()
    {
        base.Update();

        if(!player.inputManager.isBlocking)
            ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
