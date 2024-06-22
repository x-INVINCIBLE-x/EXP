using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DodgeType { DodgeRoll, DodgeStand}

public class PlayerDodgeState : PlayerState
{
    private readonly int dodgeType;
    private Vector2 dodgeDir;

    public PlayerDodgeState(PlayerStateMachine stateMachine, Player player, string animBoolName, DodgeType dodgeType, Vector2 dodgeDir) : base(stateMachine, player, animBoolName)
    {
        this.dodgeType = (int)dodgeType;
        this.dodgeDir = dodgeDir;
    }

    public override void Enter()
    {
        if (!player.stats.HasEnoughStamina(player.dodgeStamina))
            ChangeToLocomotion();

        player.anim.CrossFadeInFixedTime(animName, 0, 0);
        player.anim.SetInteger("DodgeType", dodgeType);

        if(dodgeType == 1)
        {
            player.anim.SetFloat("DodgeForward", dodgeDir.y, 0.1f, Time.deltaTime);
            player.anim.SetFloat("DodgeRight", dodgeDir.x, 0.1f, Time.deltaTime);
        }
    }

    public override void Update()
    {
        base.Update();

        Vector3 movement = new();
        movement = CalculateDodgeMovement(movement);

        Move(movement, 1);

        if (HasAnimationCompleted(animName))
            ChangeToLocomotion();
    }

    private Vector3 CalculateDodgeMovement(Vector3 movement)
    {
        if (dodgeType == 1)
        {
            movement += dodgeDir.x * player.dodgeLength * 0.075f * player.transform.right / player.dodgeDuration;
            movement += dodgeDir.y * player.dodgeLength * player.transform.forward / player.dodgeDuration;
        }
        else
        {
            movement += player.dodgeRollLength * player.transform.forward / player.dodgeDuration;
        }

        return movement;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
