using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected string animBoolName;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
