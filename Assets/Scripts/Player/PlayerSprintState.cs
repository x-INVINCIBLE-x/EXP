using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerState
{
    private float timer = 0;
    public PlayerSprintState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputManager.DodgeEvent += OnDodge;
        player.inputManager.LightAttackEvent += OnLightAttack;
        player.inputManager.HeavyAttackEvent += OnHeavyAttack;
    }

    public override void Update()
    {
        base.Update();
        MovementTimer();

        if (timer > 0.12f || (!player.inputManager.isSprinting && timer > 0.2f))
        {
            ChangeToLocomotion();
        }

        Vector3 movement = CalculateMovement();
        Move(movement, player.runSpeed);
        FreeLookDirection(movement);
    }
    public override void Exit()
    {
        base.Exit();
        player.inputManager.DodgeEvent -= OnDodge;
        player.inputManager.LightAttackEvent -= OnLightAttack;
        player.inputManager.HeavyAttackEvent -= OnHeavyAttack;
    }

    private void MovementTimer()
    {
        if (player.inputManager.Movement == Vector2.zero || !player.inputManager.isSprinting)
            timer += Time.deltaTime;
        else
            timer = 0;
    }

    private void OnDodge()
    {
        stateMachine.ChangeState(player.JumpState);
    }

    private void OnLightAttack()
    {
        Attack currentAttack = player.weaponController.currentWeapon.weaponData.sprintLightAttack;
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, currentAttack));
    }

    private void OnHeavyAttack()
    {
        Attack currentAttack = player.weaponController.currentWeapon.weaponData.sprintHeavyAttack;
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, currentAttack));
    }
}
