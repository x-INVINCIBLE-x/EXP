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
}
