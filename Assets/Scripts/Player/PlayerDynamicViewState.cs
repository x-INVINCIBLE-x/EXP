using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDynamicViewState : PlayerState
{
    public PlayerDynamicViewState(PlayerStateMachine stateMachine, Player player, string animName) : base(stateMachine, player, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.inputManager.LightAttackEvent += OnLightAttack;
        player.inputManager.HeavyAttackEvent += OnHeavyAttack;
        player.inputManager.ChargeAttackEvent += OnChargeAttack;
        player.inputManager.DodgeEvent += OnDodge;
        player.inputManager.SprintEvent += OnSprint;
        player.inputManager.BlockEvent += OnBlock;
        player.inputManager.FableArtsEvent += OnFableArt;
        player.inputManager.WeaponSwitchEvent += player.SwitchWeapon;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        player.inputManager.LightAttackEvent -= OnLightAttack;
        player.inputManager.HeavyAttackEvent -= OnHeavyAttack;
        player.inputManager.ChargeAttackEvent -= OnChargeAttack;
        player.inputManager.DodgeEvent -= OnDodge;
        player.inputManager.SprintEvent -= OnSprint;
        player.inputManager.BlockEvent -= OnBlock;
        player.inputManager.FableArtsEvent -= OnFableArt;
        player.inputManager.WeaponSwitchEvent -= player.SwitchWeapon;
    }

    protected void OnLightAttack()
    {
        Attack currentAttack = player.weaponController.SelectLightAttack();
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, currentAttack));
    }

    protected void OnHeavyAttack()
    {
        Attack currentAttack = player.weaponController.SelectHeavyAttack();
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, currentAttack));
    }

    protected void OnChargeAttack()
    {
        Attack currentAttack = player.weaponController.SelectChargeAttack();
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, currentAttack));
    }

    protected void OnDodge()
    {
        Vector2 movement = player.inputManager.Movement;

        if (movement != Vector2.zero)
            stateMachine.ChangeState(new PlayerDodgeState(stateMachine, player, "TargetDodge", DodgeType.DodgeStand, movement));
    }

    protected void OnSprint()
    {
        if (player.inputManager.Movement == Vector2.zero)
            return;

        stateMachine.ChangeState(player.SprintState);
    }

    protected void OnBlock()
    {
        ChangeToBlock();
    }
}
