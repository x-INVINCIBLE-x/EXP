using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, ISaveable
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
    public PlayerStopState stopRunState { get; private set; }
    public PlayerStopState stopWalkState { get; private set; }
    public PlayerFX fx;

    public float walkSpeed;
    public float runSpeed;
    public float rotationDamping;

    public float dodgeLength;
    public float dodgeDuration;
    public float dodgeRollLength;
    public float dodgeCooldown = 1.5f;
    public float lastTimeDodged;

    public float jumpLength;
    public float jumpDuration;

    public float jumpForce;

    public float perfectBlockTimer;

    public float freeLookTargetRadius;

    public float sprintStaminaRate;
    public float sprintStaminaThreshold;
    public float dodgeStamina;

    private float weaponSwitchCooldown = 1.5f;
    private float lastTimeWeaponswitched = -10f;

    public bool isBusy = false;
    public bool canMove = true;
    [HideInInspector] public bool isMoving = false;
    public bool canCounter = false;

    //TEMPORARY
    public Vector3 boxDimensions;
    public float maxDistance;
    public LayerMask enemyLayer;

    private void Awake()
    {
        stateMachine  = new PlayerStateMachine();

        weaponController = GetComponent<PlayerWeaponController>();
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        targeter = GetComponentInChildren<Targeter>();
        forceReciever = GetComponentInChildren<ForceReciever>();
        weaponVisiuals = GetComponent<PlayerWeaponVisuals>();
        stats = GetComponent<PlayerStat>();
        fx = GetComponent<PlayerFX>();

        mainCamera = Camera.main;
        inputManager = InputManager.Instance;
    }

    private void Start()
    {
        FreeLookState = new PlayerFreeLookState(stateMachine, this, "FreeLook");
        TargetState = new PlayerTargetState(stateMachine, this, "TargetLook");
        SprintState = new PlayerSprintState(stateMachine, this, "Sprint");
        JumpState = new PlayerJumpState(stateMachine, this, "Jump");
        stopRunState = new PlayerStopState(stateMachine, this, "Run To Stop", 0.1f);
        stopWalkState = new PlayerStopState(stateMachine, this, "Walk To Stop", 0.3f);

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

        if (weaponController.backupWeapon == null)
            return;

        if (Time.time < weaponSwitchCooldown + lastTimeWeaponswitched)
            return;

        weaponController.SwitchWeapon();// changes current weapon to backup weapon
        weaponVisiuals.SwitchWeapon(weaponController.currentWeapon);
        lastTimeWeaponswitched = Time.time;
    }

    public void ChangeWeaponModel() => weaponController.CreateWeaponModel();

    public void SetBusy(bool busy) => isBusy = busy;
    public void SetCanMove(bool move) => canMove = move;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, freeLookTargetRadius);
        Gizmos.DrawWireCube((transform.position + new Vector3(0, 1 , 2)), boxDimensions* maxDistance);
    }

    public object CaptureState()
    {
        MoveData data = new MoveData();
        data.pos = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.eulerAngles);
        return data;
    }

    public void RestoreState(object state)
    {
        MoveData data = (MoveData)state;
        Vector3 pos = data.pos.ToVector();
        Vector3 rotation = data.rotation.ToVector();
        transform.position = pos;
        transform.eulerAngles = rotation;
    }

    [System.Serializable]
    struct MoveData
    {
       public SerializableVector3 pos;
       public SerializableVector3 rotation;
    }
}
