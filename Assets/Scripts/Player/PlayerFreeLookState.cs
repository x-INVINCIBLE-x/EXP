using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerFreeLookState : PlayerDynamicViewState
{
    private readonly int FreeLookHash = Animator.StringToHash("FreeLookSpeed");

    private float transitionDuration;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, Player player, string animBoolName, float transitionDuration = 0.5f) : base(stateMachine, player, animBoolName)
    {
        this.transitionDuration = transitionDuration;
    }

    public override void Enter()
    {
        base.Enter();

        player.anim.CrossFadeInFixedTime(animName, transitionDuration, 0);
        player.inputManager.DodgeEvent += OnDodge;
        player.inputManager.TargetEvent += OnTarget;
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
        player.inputManager.DodgeEvent -= OnDodge;
        player.inputManager.TargetEvent -= OnTarget;
    }

    private void OnDodge()
    {
        if (Time.time < player.lastTimeDodged + player.dodgeCooldown)
            return;

        player.lastTimeDodged = Time.time;
        Vector2 movement = player.inputManager.Movement;

        if (movement != Vector2.zero)
            stateMachine.ChangeState(new PlayerDodgeState(stateMachine, player, "FreeDodge", DodgeType.DodgeRoll, movement));
    }

    private void OnTarget()
    {
        if (!player.targeter.SelectTarget()) return;

        stateMachine.ChangeState(player.TargetState);
    }
}
