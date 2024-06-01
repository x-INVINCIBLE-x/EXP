using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerStateMachine stateMachine, Player player, string animName) : base(stateMachine, player, animName)
    {
    }

    public override void Enter()
    {
        player.anim.CrossFadeInFixedTime(animName, 0.125f, 0, 0.1f);
    }

    public override void Update()
    {
        base.Update();
        if(IsAnimationComplete(animName))
            ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
