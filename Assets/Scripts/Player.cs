using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected PlayerStateMachine StateMachine { get; private set; }
    public Animator anim {  get; private set; }

    public PlayerFreeLookState IdleState { get; private set; }

    private void Awake()
    {
        StateMachine  = new PlayerStateMachine();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        IdleState = new PlayerFreeLookState(StateMachine, this, "Idle");

        StateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        StateMachine.currentState.Update();
    }
}
