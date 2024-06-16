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
    public PlayerStat stats { get; private set; }
    public Camera mainCamera { get; private set; }

    public PlayerFreeLookState FreeLookState { get; private set; }
    public PlayerTargetState TargetState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerWeaponController weaponController { get; private set; }
    public PlayerWeaponVisuals weaponVisiuals { get; private set; }

    public float walkSpeed;
    public float runSpeed;
    public float rotationDamping;

    public float dodgeLength;
    public float dodgeDuration;
    public float dodgeRollLength;

    public float jumpLength;
    public float jumpDuration;

    public float jumpForce;

    private float weaponSwitchCooldown = 1.5f;
    private float lastTimeWeaponswitched = -10f;

    public bool isBusy = false;

    private void Awake()
    {
        stateMachine  = new PlayerStateMachine();

        weaponController = GetComponent<PlayerWeaponController>();
        inputManager = GetComponent<InputManager>();
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        targeter = GetComponentInChildren<Targeter>();
        forceReciever = GetComponentInChildren<ForceReciever>();
        weaponVisiuals = GetComponent<PlayerWeaponVisuals>();
        stats = GetComponent<PlayerStat>();

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
        if (isBusy)
            return;

        if (Time.time < weaponSwitchCooldown + lastTimeWeaponswitched)
            return;

        weaponController.SwitchWeapon();// changes current weapon to backup weapon
        weaponVisiuals.SwitchWeapon(weaponController.currentWeapon);
        lastTimeWeaponswitched = Time.time;
    }

    public void ChangeWeaponModel() => weaponController.ChangeWeaponModel();

    public void SetBusy(bool busy) => isBusy = busy;
}
