using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public InputManager inputManager { get; private set; }
    public Animator anim {  get; private set; }
    public CharacterController characterController { get; private set; }
    public Targeter targeter { get; private set; }
    public Camera mainCamera { get; private set; }

    public PlayerFreeLookState FreeLookState { get; private set; }
    public PlayerTargetState TargetState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }

    public float walkSpeed;
    public float runSpeed;
    public float rotationDamping;

    private void Awake()
    {
        stateMachine  = new PlayerStateMachine();
        inputManager = GetComponent<InputManager>();
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        targeter = GetComponentInChildren<Targeter>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        FreeLookState = new PlayerFreeLookState(stateMachine, this, "FreeLook");
        TargetState = new PlayerTargetState(stateMachine, this, "TargetLook");
        SprintState = new PlayerSprintState(stateMachine, this, "Sprint");

        stateMachine.InitializeState(FreeLookState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
