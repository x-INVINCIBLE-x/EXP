using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFableArtState : PlayerState
{
    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, string animName) : base(stateMachine, player, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.anim.CrossFadeInFixedTime(animName, 0.05f, 0);
    }

    public override void Update()
    {
        base.Update();

        if (HasAnimationCompleted(animName))
            ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
