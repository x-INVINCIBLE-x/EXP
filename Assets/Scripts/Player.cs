using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected PlayerStateMachine StateMachine { get; private set; }
    public Animator anim {  get; private set; }

    public PlayerIdleState IdleState { get; private set; }

    private void Awake()
    {
        StateMachine  = new PlayerStateMachine();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        IdleState = new PlayerIdleState(StateMachine, this, "Idle");

        StateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        StateMachine.currentState.Update();
    }
}
