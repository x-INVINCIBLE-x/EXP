using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DodgeType { DodgeRoll, DodgeStand}

public class PlayerDodgeState : PlayerState
{
    private int dodgeType;
    private Vector3 dodgeDir;

    public PlayerDodgeState(PlayerStateMachine stateMachine, Player player, string animBoolName, DodgeType dodgeType, Vector3 dodgeDir) : base(stateMachine, player, animBoolName)
    {
        this.dodgeType = (int)dodgeType;
        this.dodgeDir = dodgeDir;
    }

    public override void Enter()
    {
        base.Enter();
        player.anim.SetInteger("DodgeType", dodgeType);

        if(dodgeType == 1)
        {
            player.anim.SetFloat("DodgeForward", dodgeDir.x);
            player.anim.SetFloat("DodgeRight", dodgeDir.y);
        }
    }

    public override void Update()
    {
        base.Update();

        if(IsAnimationComplete("Free Dodge") || IsAnimationComplete("Tadrget Dodge"))
                ChangeToLocomotion();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
