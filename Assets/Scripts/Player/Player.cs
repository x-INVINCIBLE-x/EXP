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
    public ForceReciever forceReciever { get; private set; }
    public Camera mainCamera { get; private set; }

    public PlayerFreeLookState FreeLookState { get; private set; }
    public PlayerTargetState TargetState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerWeaponController weaponController { get; private set; }
    public PlayerWeaponVisuals weaponVisiuals { get; private set; }

    public Transform weaponHolder;

    public float walkSpeed;
    public float runSpeed;
    public float rotationDamping;

    public float dodgeLength;
    public float dodgeDuration;
    public float dodgeRollLength;

    public float jumpLength;
    public float jumpDuration;

    public float jumpForce;

    #region TEMP
    public WeaponData currentWeapon;
    public WeaponData backupWeapon;
    #endregion

    private void Awake()
    {
        stateMachine  = new PlayerStateMachine();
        weaponController = new PlayerWeaponController();
        weaponController.currentWeapon = currentWeapon;
        weaponController.backupWeapon = backupWeapon;

        inputManager = GetComponent<InputManager>();
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        targeter = GetComponentInChildren<Targeter>();
        forceReciever = GetComponentInChildren<ForceReciever>();
        weaponVisiuals = GetComponent<PlayerWeaponVisuals>();

        mainCamera = Camera.main;
    }

    private void Start()
    {
        FreeLookState = new PlayerFreeLookState(stateMachine, this, "FreeLook");
        TargetState = new PlayerTargetState(stateMachine, this, "TargetLook");
        SprintState = new PlayerSprintState(stateMachine, this, "Sprint");
        JumpState = new PlayerJumpState(stateMachine, this, "Jump");

        stateMachine.InitializeState(FreeLookState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void SwitchWeapon()
    {
        weaponController.SwitchWeapon();
        weaponVisiuals.SwitchWeapon(weaponController.backupWeapon);
    }
}
