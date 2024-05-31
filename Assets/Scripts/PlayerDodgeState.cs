using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DodgeType { DodgeRoll, DodgeStand}

public class PlayerDodgeState : PlayerState
{
    private int dodgeType;
    private Vector2 dodgeDir;

    public PlayerDodgeState(PlayerStateMachine stateMachine, Player player, string animBoolName, DodgeType dodgeType, Vector2 dodgeDir) : base(stateMachine, player, animBoolName)
    {
        this.dodgeType = (int)dodgeType;
        this.dodgeDir = dodgeDir;
    }

    public override void Enter()
    {
        player.anim.CrossFadeInFixedTime(animBoolName, 0, 0);
        player.anim.SetInteger("DodgeType", dodgeType);
        Debug.Log(dodgeDir);
        if(dodgeType == 1)
        {
            player.anim.SetFloat("DodgeForward", dodgeDir.y);
            player.anim.SetFloat("DodgeRight", dodgeDir.x);
        }
    }

    public override void Update()
    {
        base.Update();

        if(IsAnimationComplete(animBoolName))
                ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
