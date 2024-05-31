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

    public void Move(Vector3 movement, float speed)
    {
        player.characterController.Move(speed * Time.deltaTime * movement);
    }

    public void FaceTarget()
    {
        Target target = player.targeter.currentTarget;
        if (target == null) return;

        Vector3 dir = (player.transform.position - target.transform.position).normalized;
        dir.y = 0;

        player.transform.rotation = Quaternion.LookRotation(-dir);
    }

    public void Run(Vector3 movement)
    {
        player.anim.SetBool("Run", true);
        Move(movement, player.runSpeed);
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
}
