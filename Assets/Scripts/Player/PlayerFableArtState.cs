using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerFableArtState : PlayerState
{
    private readonly int animationSpeedHash = Animator.StringToHash("AttackSpeedMultiplier");
    private float animationSpeedMultiplier;

    private Vector3 movement;
    private Attack attack;
    private float stateTimer = 0.2f;
   
    private EffectData effect;
    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, string animName, Attack attack = null, float animationSpeedMultiplier = 1f, EffectData effect = null) : base(stateMachine, player, animName)
    {
        this.attack = attack;
        this.animationSpeedMultiplier = animationSpeedMultiplier;
        this.effect = effect;
    }

    public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, Attack attack, float animationSpeedMultiplier = 1f, EffectData effect = null) : this(stateMachine, player, null, attack, animationSpeedMultiplier, effect) { }
    //public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, string animName, float animationSpeedMultiplier, EffectData effect = null) : this(stateMachine, player, animName, null, animationSpeedMultiplier, effect) { }
    //public PlayerFableArtState(PlayerStateMachine stateMachine, Player player, string animName, float animationSpeedMultiplier, EffectData effect) : this(stateMachine, player, animName, null, animationSpeedMultiplier, effect) { }
    public override void Enter()
    {
        //base.Enter();

        stateTimer = 0.2f;
        ChooseAttackAnimation();

        if (animationSpeedMultiplier > 1f)
            player.anim.SetFloat(animationSpeedHash, animationSpeedMultiplier);

        if (player.inputManager.Movement != Vector2.zero)
            movement = player.inputManager.Movement;
        else
            movement = player.transform.forward;

        player.fx.StartEffect(effect);


        StopMovement();
        player.SetCanMove(true);
    }

    public override void Update()
    {
        base.Update();

        SetFacingDirection(movement);
        if (attack != null)
        {
            Move(movement, attack.movementSpeed);
            if (HasAnimationCompleted(attack.AnimationName))
                ChangeToLocomotion();

            return;
        }
        
        if (HasAnimationCompleted(animName)) //IDK ,Never Gets called, but attack can be null -_- (DED)
            ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();

        player.SetCanMove(true);
        player.anim.SetFloat(animationSpeedHash, 1f);
        player.fx.RemoveEffect(effect);
    }

    private void ChooseAttackAnimation()
    {
        if (attack != null)
            player.anim.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionTime, 0);
    }


    private void SetFacingDirection(Vector3 movement)
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer < 0)
            return;

        if (movement != Vector3.zero)
        {
            FreeLookDirection(movement);
        }
    }

}
