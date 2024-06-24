using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private Attack attack;
    private Vector2 movement;

    private float animationSpeedMultiplier;
    private readonly int animationSpeedHash = Animator.StringToHash("AttackSpeedMultiplier");
    public PlayerAttackState(PlayerStateMachine stateMachine, Player player, Attack attack, float animationMultiplier = 1f) : base(stateMachine, player, attack.AnimationName)
    {
        this.attack = attack;
        animationSpeedMultiplier = animationMultiplier;
    }

    public override void Enter()
    {
        FaceClosestTarget();

        player.stats.physicalAtk.AddModifier(new StatModifier(attack.PhysicalATK,  StatModType.Flat, attack));

        player.anim.CrossFadeInFixedTime(animName, attack.TransitionTime, 0, 0f);
        player.anim.SetFloat(animationSpeedHash, animationSpeedMultiplier);

        player.inputManager.LightAttackEvent += OnLIghtAttack;
        player.inputManager.HeavyAttackEvent += OnHeavyAttack;
        player.inputManager.ChargeAttackEvent += OnChargeAttack;

        movement = player.inputManager.Movement;

        StopMovement();
        player.stats.SetConsumingStamina(true);
    }

    public override void Update()
    {
        base.Update();

        if(!HasAnimationPassed(animName, 0.8f))
            Move(player.transform.forward, attack.movementSpeed);

        if (HasAnimationCompleted(animName))
            ChangeToLocomotion(0.3f);

        if (HasAnimationPassed(animName, 0.9f) && movement.sqrMagnitude != 0)
            ChangeToLocomotion(0.3f);
    }

    public override void Exit()
    {
        base.Exit();

        player.stats.physicalAtk.RemoveAllModifiersFromSource(attack);

        player.anim.SetFloat(animationSpeedHash, 1f);

        player.inputManager.LightAttackEvent -= OnLIghtAttack;
        player.inputManager.HeavyAttackEvent -= OnHeavyAttack;
        player.inputManager.ChargeAttackEvent -= OnChargeAttack;
        player.stats.SetConsumingStamina(false);
    }

    private void FaceClosestTarget()
    {
        if (player.targeter.currentTarget != null)
            return;
        
        Target target = player.targeter.GetClosestTarget();

        if (target == null)
            return;

        FaceTarget(target);
    }

    private void OnLIghtAttack()
    {
        if (!HasAnimationPassed(animName, 0.9f))
            return;

        Attack nextAttack = player.weaponController.SelectLightAttack();

        InitiateNextAttack(nextAttack);
    }

    private void OnHeavyAttack()
    {
        if (!HasAnimationPassed(animName, 0.9f))
            return;

        Attack nextAttack = player.weaponController.SelectHeavyAttack();
        InitiateNextAttack(nextAttack);
    }

    private void OnChargeAttack()
    {
        if (!HasAnimationPassed(animName, 0.9f))
            return;

        Attack nextAttack = player.weaponController.SelectChargeAttack();
        InitiateNextAttack(nextAttack);
    }

    private void InitiateNextAttack(Attack nextAttack)
    {
        if (nextAttack != null)
            stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, nextAttack));
    }
}
