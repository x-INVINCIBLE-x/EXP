using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFableArtState : PlayerState
{
    private Vector3 movement;
    private Attack attack;

    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, string animName, Attack attack = null) : base(stateMachine, player, animName)
    {
        this.attack = attack;
    }

    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, Attack attack) : this(stateMachine, player, null, attack) { }

    public override void Enter()
    {
        base.Enter();
        if(attack != null) 
            player.anim.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionTime, 0);
        else
            player.anim.CrossFadeInFixedTime(animName, 0.05f, 0);

        movement = player.inputManager.Movement;
        StopMovement();
    }

    public override void Update()
    {
        base.Update();

        if(attack != null)
        {
            Move(movement, attack.movementSpeed);
            if(HasAnimationCompleted(attack.AnimationName))
                ChangeToLocomotion();

            return;
        }

        if (HasAnimationCompleted(animName))
            ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
