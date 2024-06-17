using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected string animName;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        player.anim.CrossFadeInFixedTime(animName, 0.5f, 0);
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }

    public void Move(Vector3 movement, float speed = 1)
    {
        if (player.isBusy)
            return;

        player.characterController.Move(((speed * movement) + player.forceReciever.Movement) * Time.deltaTime);
    }

    protected Vector3 CalculateMovement()
    {
        Vector3 forward = (player.mainCamera.transform.forward).normalized;
        Vector3 right = (player.mainCamera.transform.right).normalized;

        forward.y = 0;
        right.y = 0;

        return (player.inputManager.Movement.x * right) + (player.inputManager.Movement.y * forward);
    }

    protected void StopMovement()
    {
        player.characterController.Move(Vector3.zero);
    }

    public void FaceTarget()
    {
        Target target = player.targeter.currentTarget;
        if (target == null) return;

        Vector3 dir = (player.transform.position - target.transform.position).normalized;
        dir.y = 0;

        player.transform.rotation = Quaternion.LookRotation(-dir);
    }

    protected void FreeLookDirection(Vector3 movement)
    {
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * player.rotationDamping);
    }

    protected void ChangeToLocomotion(float transitionDuration = 0.5f)
    {
        if (player.targeter.currentTarget == null)
        {
            //stateMachine.ChangeState(player.FreeLookState);
            stateMachine.ChangeState(new PlayerFreeLookState(stateMachine, player, "FreeLook", transitionDuration));
        }
        else
        {
            //stateMachine.ChangeState(player.TargetState);
            stateMachine.ChangeState(new PlayerTargetState(stateMachine, player, "TargetLook", transitionDuration));
        }
    }

    protected bool HasAnimationCompleted(string animationName) => HasAnimationPassed(animationName, 1f);
 
    protected bool HasAnimationPassed(string animationName, float normalizedTime)
    {
        AnimatorStateInfo stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            if (stateInfo.normalizedTime >= normalizedTime)
            {
                return true;
            }
        }

        return false;
    }

    protected void OnFableArt()
    {
        player.weaponController.FableAttack();
    }

    public void ChangeToBlock()
    {
        if (player.canCounter)
        {
            AnimationClip counterClip = player.weaponController.currentWeapon.counterAttackAnim;
            string counterAttackName = counterClip == null ? "Universal Counter" : counterClip.name;
            stateMachine.ChangeState(new PlayerBlockState(stateMachine, player, "Standing Block Idle", counterAttackName));
            Debug.Log("Counter Block");
            return;
        }

        Debug.Log("Simple Block");
        stateMachine.ChangeState(new PlayerBlockState(stateMachine, player, "Standing Block Idle"));
    }
}
