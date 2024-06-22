using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetState : PlayerDynamicViewState
{
    private readonly int TargetForwardHash = Animator.StringToHash("TargetForward");
    private readonly int TargetRightHash = Animator.StringToHash("TargetRight");

    private float transitionDuration;

    public PlayerTargetState(PlayerStateMachine stateMachine, Player player, string animBoolName, float transitionDuration = 0.5f) : base(stateMachine, player, animBoolName)
    {
        this.transitionDuration = transitionDuration;
    }

    public override void Enter()
    {
        base.Enter();

        player.anim.CrossFadeInFixedTime(animName, transitionDuration, 0);
        player.inputManager.DodgeEvent += OnDodge;
        player.inputManager.TargetEvent += OnCancelTarget;
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

        player.inputManager.DodgeEvent -= OnDodge;
        player.inputManager.TargetEvent -= OnCancelTarget;
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

    private void OnDodge()
    {
        Vector2 movement = player.inputManager.Movement;

        if (movement != Vector2.zero)
            stateMachine.ChangeState(new PlayerDodgeState(stateMachine, player, "TargetDodge", DodgeType.DodgeStand, movement));
    }

    private void OnCancelTarget()
    {
        if(player.targeter.currentTarget != null)
            player.targeter.RemoveCurrentTarget();

        stateMachine.ChangeState(player.FreeLookState);
    }
}
