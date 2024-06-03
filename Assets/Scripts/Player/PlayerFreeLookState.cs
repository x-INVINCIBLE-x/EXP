using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerFreeLookState : PlayerState
{
    private readonly int FreeLookHash = Animator.StringToHash("FreeLookSpeed");
    public PlayerFreeLookState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputManager.TargetEvent += OnTarget;
        player.inputManager.SprintEvent += OnSprint;
        player.inputManager.DodgeEvent += OnDodge;
        player.inputManager.LightAttackEvent += OnLightAttack;
        player.inputManager.HeavyAttackEvent += OnHeavyAttack;
        player.inputManager.ChargeAttackEvent += OnChargeAttack;
    }

    public override void Update()
    {
        base.Update();

        Vector3 movement = CalculateMovement();
        if (movement != Vector3.zero)
        {
            FreeLookDirection(movement);

            player.anim.SetFloat(FreeLookHash, 1f, 0.1f, Time.deltaTime);
            Move(movement, player.walkSpeed);
        }
        else
            player.anim.SetFloat(FreeLookHash, 0, 0.15f, Time.deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
        player.inputManager.TargetEvent -= OnTarget;
        player.inputManager.SprintEvent -= OnSprint;
        player.inputManager.DodgeEvent -= OnDodge;
        player.inputManager.LightAttackEvent -= OnLightAttack;
        player.inputManager.HeavyAttackEvent -= OnHeavyAttack;
        player.inputManager.ChargeAttackEvent -= OnChargeAttack;
    }

    private void OnTarget()
    {
        if (!player.targeter.SelectTarget()) return;

        stateMachine.ChangeState(player.TargetState);
    }

    private void OnSprint()
    {
        if(player.inputManager.Movement == Vector2.zero)
            return;

        stateMachine.ChangeState(player.SprintState);
    }

    private void OnDodge()
    {
        Vector2 movement = player.inputManager.Movement;

        if(movement != Vector2.zero) 
            stateMachine.ChangeState(new PlayerDodgeState(stateMachine, player, "FreeDodge", DodgeType.DodgeRoll, movement));
    }

    private void OnLightAttack()
    {
        Attack currentAttack = player.weaponController.SelectLightAttack();
        stateMachine.ChangeState(new PlayerAttackState(stateMachine, player, currentAttack));
    }

    private void OnHeavyAttack()
    {

    }

    private void OnChargeAttack()
    {

    }
}
