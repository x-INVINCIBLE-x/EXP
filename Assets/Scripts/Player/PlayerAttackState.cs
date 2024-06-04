using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private Attack attack;
    private Vector2 movement;

    public PlayerAttackState(PlayerStateMachine stateMachine, Player player, Attack attack) : base(stateMachine, player, attack.AnimationName)
    {
        this.attack = attack;
    }

    public override void Enter()
    {
        player.anim.CrossFadeInFixedTime(animName, attack.TransitionTime, 0);

        player.inputManager.LightAttackEvent += OnLIghtAttack;
        player.inputManager.HeavyAttackEvent += OnHeavyAttack;
        player.inputManager.ChargeAttackEvent += OnChargeAttack;

        movement = player.inputManager.Movement;

        StopMovement();
    }

    public override void Update()
    {
        base.Update();

        if(!HasAnimationPassed(animName, 0.8f))
            Move(player.transform.forward, attack.movementSpeed);

        if (IsAnimationComplete(animName))
            ChangeToLocomotion();

        if (HasAnimationPassed(animName, 0.9f) && movement.sqrMagnitude != 0)
            ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();

        player.inputManager.LightAttackEvent -= OnLIghtAttack;
        player.inputManager.HeavyAttackEvent -= OnHeavyAttack;
        player.inputManager.ChargeAttackEvent -= OnChargeAttack;
    }

    private void OnLIghtAttack()
    {
        if (!HasAnimationPassed(animName, 0.9f))
            return; 

        Attack nextAttack = player.weaponController.SelectLightAttack();
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, nextAttack));
    }

    private void OnHeavyAttack()
    {
        if (!HasAnimationPassed(animName, 0.9f))
            return;

        Attack nextAttack = player.weaponController.SelectHeavyAttack();
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, nextAttack));
    }

    private void OnChargeAttack()
    {

    }
}
