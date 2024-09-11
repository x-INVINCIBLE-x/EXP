using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopState : PlayerState
{
    private float transitionTime;
    public PlayerStopState(PlayerStateMachine stateMachine, Player player, string animName, float transitionTIme = 1f) : base(stateMachine, player, animName)
    {
        this.transitionTime = transitionTIme;
    }

    public override void Enter()
    {
        if (player.targeter.currentTarget != null)
            FaceTarget(player.targeter.currentTarget);
        player.anim.CrossFadeInFixedTime(animName, transitionTime, 0 , 0f);
        player.isMoving = false;
    }

    public override void Update()
    {
        base.Update();

        Vector3 movement = CalculateMovement();
        Move(movement);

        if ((HasAnimationPassed(animName, 0.3f) && movement.sqrMagnitude != 0) || HasAnimationCompleted(animName))
        {
                ChangeToLocomotion(0.3f);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
