using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFableArtState : PlayerState
{
    private Vector3 movement;
    private Attack attack;
    private float animationSpeedMultiplier;
    private readonly int animationSpeedHash = Animator.StringToHash("AttackSpeedMultiplier");

    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, string animName, Attack attack = null, float animationSpeedMultiplier = 1f) : base(stateMachine, player, animName)
    {
        this.attack = attack;
        this.animationSpeedMultiplier = animationSpeedMultiplier;
    }

    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, Attack attack, float animationSpeedMultiplier = 1f) : this(stateMachine, player, null, attack, animationSpeedMultiplier) { }
    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, string animName, float animationSpeedMultiplier): this(stateMachine, player, animName, null, animationSpeedMultiplier) { }
    public override void Enter()
    {
        base.Enter();
        if(attack != null) 
            player.anim.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionTime, 0);
        else
            player.anim.CrossFadeInFixedTime(animName, 0.05f, 0);

        player.anim.SetFloat(animationSpeedHash, animationSpeedMultiplier);

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

        player.anim.SetFloat(animationSpeedHash, 1f);
    }
}
