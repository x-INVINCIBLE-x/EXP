using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    Vector3 momentum;
    public PlayerFallState(PlayerStateMachine stateMachine, Player player, string animName) : base(stateMachine, player, animName)
    {
    }

    public override void Enter()
    {
        player.anim.CrossFadeInFixedTime(animName, 0.125f, 0, 0.1f);


        player.forceReciever.Jump(player.jumpForce);
        momentum = player.characterController.velocity;
        momentum.y = 0;
    }

    public override void Update()
    {
        base.Update();
        momentum = Vector3.Lerp(momentum, Vector3.zero, 3 * Time.deltaTime);
        Move(momentum);

        if(HasAnimationCompleted(animName))
            ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
