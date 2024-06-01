using System.Collections;
using System.Collections.Generic;
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
        player.characterController.Move(((speed * movement) + player.forceReciever.Movement) * Time.deltaTime);
    }

    public void FaceTarget()
    {
        Target target = player.targeter.currentTarget;
        if (target == null) return;

        Vector3 dir = (player.transform.position - target.transform.position).normalized;
        dir.y = 0;

        player.transform.rotation = Quaternion.LookRotation(-dir);
    }

    protected Vector3 CalculateMovement()
    {
        Vector3 forward = (player.mainCamera.transform.forward).normalized;
        Vector3 right = (player.mainCamera.transform.right).normalized;

        forward.y = 0;
        right.y = 0;

        return (player.inputManager.Movement.x * right) + (player.inputManager.Movement.y * forward);
    }

    protected void FreeLookDirection(Vector3 movement)
    {
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * player.rotationDamping);
    }

    protected void ChangeToLocomotion()
    {
        if (player.targeter.currentTarget == null)
        {
            stateMachine.ChangeState(player.FreeLookState);
        }
        else
        {
            stateMachine.ChangeState(player.TargetState);
        }
    }

    protected bool IsAnimationComplete(string animationName)
    {
        AnimatorStateInfo stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            if (stateInfo.normalizedTime >= 1f)
            {
                return true;
            }
        }

        return false;
    }
}
