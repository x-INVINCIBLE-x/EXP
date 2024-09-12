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
    private bool isPerfectBlock;
    private string counterAnimName;

    public PlayerBlockState(PlayerStateMachine stateMachine, Player player, string animName, float blockTime = 0, bool isPerfectBlockOnly = false) : base(stateMachine, player, animName)
    {
        this.blockTime = blockTime;
        this.isPerfectBlockOnly = isPerfectBlockOnly;
    }

    public PlayerBlockState(PlayerStateMachine stateMachine, Player player, string animName, string counterAnimName) : this(stateMachine, player, animName)
    {
        this.counterAnimName = counterAnimName;
    }

    public override void Enter()
    {
        base.Enter();

        player.stats.physicalDef.AddModifier(new StatModifier(player.weaponController.currentWeapon.guardAmount, StatModType.Flat, this));

        player.stats.UpdateHUD += OnHit;

        perfectblockTimer = player.perfectBlockTimer;

        player.stats.SetBlocking(true);
        player.stats.SetPerfectBlock(true);
        isPerfectBlockTriggered = false;
        isPerfectBlock = true;

        Debug.Log("IsPerfectBlocking");

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

        if (!isPerfectBlock)
            return;

        perfectblockTimer -= Time.deltaTime;

        if (perfectblockTimer < 0f && !isPerfectBlockTriggered)
        {
            isPerfectBlock = false;
            player.stats.SetPerfectBlock(false);
            isPerfectBlockTriggered = true;

            Debug.Log("IsPerfectBlockingOver");
        }

    }

    public override void Exit()
    {
        base.Exit();

        player.stats.physicalDef.RemoveAllModifiersFromSource(this);

        player.stats.UpdateHUD -= OnHit;

        player.stats.SetBlocking(false);
        player.stats.SetPerfectBlock(false);
    }

    private void OnHit()
    {
        if (!isPerfectBlock)
            return;

        if (string.IsNullOrEmpty(counterAnimName))
            return;

        stateMachine.ChangeState(new PlayerFableArtState(stateMachine, player, counterAnimName));
    }
}
