using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetState : PlayerState
{
    private readonly int TargetForwardHash = Animator.StringToHash("TargetForward");
    private readonly int TargetRightHash = Animator.StringToHash("TargetRight");

    public PlayerTargetState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputManager.SprintEvent += OnSprint;
    }

    public override void Update()
    {
        base.Update();

        Vector3 movement = CalculateMovement();
        UpdateAnimation();
        if (movement != Vector3.zero)
        {
            Move(movement, player.walkSpeed);
            FaceTarget(); 
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.inputManager.SprintEvent -= OnSprint;
    }

    private void UpdateAnimation()
    {
        float value;
        if (Mathf.Approximately(player.inputManager.Movement.y, 0f))
        {
            value = 0;
        }
        else
        {
            value = player.inputManager.Movement.y > 0 ? 1f : -1f;
        }

        player.anim.SetFloat(TargetForwardHash, value, 0.1f, Time.deltaTime);

        if (Mathf.Approximately(player.inputManager.Movement.x, 0f))
        {
            value = 0;
        }
        else
        {
            value = player.inputManager.Movement.x > 0 ? 1f : -1f;
        }

        player.anim.SetFloat(TargetRightHash, value, 0.1f, Time.deltaTime);
    }

    private void OnSprint()
    {
        if (player.inputManager.Movement == Vector2.zero)
            return;

        stateMachine.ChangeState(player.SprintState);
    }
}
