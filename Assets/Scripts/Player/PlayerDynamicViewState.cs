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
        player.inputManager.SprintEvent -= OnSprint;
        player.inputManager.BlockEvent -= OnBlock;
        player.inputManager.FableArtsEvent -= OnFableArt;
        player.inputManager.WeaponSwitchEvent -= player.SwitchWeapon;
    }

    protected void OnLightAttack()
    {
        Attack currentAttack = player.weaponController.SelectLightAttack();
        InitiateAttack(currentAttack);
    }


    protected void OnHeavyAttack()
    {
        Attack currentAttack = player.weaponController.SelectHeavyAttack();
        InitiateAttack(currentAttack);
    }

    protected void OnChargeAttack()
    {
        Attack currentAttack = player.weaponController.SelectChargeAttack();
        InitiateAttack(currentAttack);
    }

    private void InitiateAttack(Attack currentAttack)
    {
        if (currentAttack != null)
            stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, currentAttack));
    }

    protected void OnSprint()
    {
        if (player.inputManager.Movement == Vector2.zero)
            return;

        if (player.staminaThreshold <= player.stats.GetCurrentStamina())
            stateMachine.ChangeState(player.SprintState);
    }

    protected void OnBlock()
    {
        ChangeToBlock();
    }
}
