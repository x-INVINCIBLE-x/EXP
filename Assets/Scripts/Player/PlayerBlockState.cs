using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerState
{
    private float blockTime;
    private bool hasTimer;
    private float perfectblockTimer;
    private bool isPerfectBlockTriggered;
    private bool isPerfectBlockOnly;

    public PlayerBlockState(PlayerStateMachine stateMachine, Player player, string animName, float blockTime = 0, bool isPerfectBlockOnly = false) : base(stateMachine, player, animName)
    {
        this.blockTime = blockTime;
        this.isPerfectBlockOnly = isPerfectBlockOnly;
    }

    public PlayerBlockState(PlayerStateMachine stateMachine, Player player, string animName, string counterAnimName) : this(stateMachine, player, animName) { }

    public override void Enter()
    {
        base.Enter();

        perfectblockTimer = 1f;
        player.stats.SetBlocking(true);
        player.stats.SetPerfectBlock(true);
        isPerfectBlockTriggered = false;

        hasTimer = blockTime > 0.1f;

        StopMovement();
        player.anim.CrossFadeInFixedTime(animName, 0.05f, 0);
    }

    public override void Update()
    {
        base.Update();
        
        CheckForPerfectBlock();

        if (hasTimer)
        {
            blockTime -= Time.deltaTime;
            if (blockTime < 0)
            {
                ChangeToLocomotion();
            }
            return;
        }


        if (!player.inputManager.isBlocking)
            ChangeToLocomotion();
    }

    private void CheckForPerfectBlock()
    {
        if(isPerfectBlockOnly)
            return;

        perfectblockTimer -= Time.deltaTime;

        if (perfectblockTimer < 0f && !isPerfectBlockTriggered)
        {
            player.stats.SetPerfectBlock(false);
            isPerfectBlockTriggered = true;
        }

    }

    public override void Exit()
    {
        base.Exit();

        player.stats.SetBlocking(false);
        player.stats.SetPerfectBlock(false);
    }
}
