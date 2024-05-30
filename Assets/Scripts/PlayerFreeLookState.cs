using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerFreeLookState : PlayerState
{
    public PlayerFreeLookState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        Vector3 movement = CalculateMovement();
        if (movement != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.Space))
                player.anim.SetFloat("FreeLookSpeed", 1f, 0.1f, Time.deltaTime);
            else
                player.anim.SetFloat("FreeLookSpeed", 0.5f, 0.1f, Time.deltaTime);
        }
        else
            player.anim.SetFloat("FreeLookSpeed", 0, 0.15f, Time.deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = (player.mainCamera.transform.forward).normalized;
        Vector3 right = (player.mainCamera.transform.right).normalized;

        forward.y = 0;
        right.y = 0;

        return (player.inputManager.Movement.x * right) + (player.inputManager.Movement.y * forward);
    }
}
