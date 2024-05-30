using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public InputManager inputManager { get; private set; }
    public Animator anim {  get; private set; }
    public CharacterController characterController { get; private set; }
    public Camera mainCamera { get; private set; }

    public PlayerFreeLookState IdleState { get; private set; }

    public float moveSpeed;
    public float runSpeed;
    public float rotationDamping;

    private void Awake()
    {
        stateMachine  = new PlayerStateMachine();
        inputManager = GetComponent<InputManager>();
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        IdleState = new PlayerFreeLookState(stateMachine, this, "FreeLook");

        stateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
