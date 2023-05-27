using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AkarisuMD.Player;
using NaughtyAttributes;
using UnityEngine.AI;

/// <summary>
/// tps player using starter pack model.
/// /!\ made with a singleton /!\
/// </summary>
// [RequireComponent(typeof(CharacterController))]
public class Tps_PlayerController : Singleton<Tps_PlayerController>
{

    //==============================================================================================================
    #region PUBLIC
    //==============================================================================================================

    /// <summary>
    /// Scritable singleton for storing player data.
    /// </summary>
    public ScS_PlayerData playerData;

    /// <summary>
    /// How movement of hunter is.
    /// </summary>
    public enum HunterMoveType
    {
        NORMAL,
        STOP,
    }
    public Tps_Player_Inputs Inputs { get { return _inputs; } }

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region REFERENCE
    //==============================================================================================================
    // [SerializeField] private CharacterController _CharacterController;
    [SerializeField] private GameObject _Body;
    [SerializeField] private Animator _Animator;
    private Camera _Camera;
    [SerializeField] private GameObject _targetCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private Transform _flashlightRoot;

    [SerializeField] private Equipment _equipment1, _equipment2;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PRIVATE
    //==============================================================================================================
    private Tps_Player_Inputs _inputs;

    [SerializeField] private sc_Weapon weapon;

    [Space(5), Header("Machine state")]
    [SerializeField] private StateMachine<Tps_PlayerController> stateMachine;

    //input
    [SerializeField] private Vector2 movementInput;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDDodge;
    private int _animIDAtk1;
    private int _animIDAtk2;
    private int _animIDHealing;
    private int _animIDGetHit;
    private int _animIDDeath;
    private int _animIDRevive;

    private float _animSpeedBlend;

    private float _movementTargetRotation = 0.0f;
    private float _lookTargetRotation = 0.0f;

    private const float Rad2Deg = 57.29578f;

    private Medkit_Script currentMedkit;

    private float revivingTimer;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region MONOBEHAVIOUR
    //==============================================================================================================

    private void Awake()
    {
        // create inputs.
        _inputs = new();
        _Camera = Camera.main;
    }

    private void OnEnable()
    {
        // active inputs
        //_inputs.Enable();
    }

    private void Start()
    {
        ResetPlayerData();

        SetVirtualCamParameters();

        playerData.monitor.isValid = true;
    }

    private void Update()
    {
        CheckStates();

        // gather all input of player.
        GatherInput();

        if (!playerData.monitor.isValid) return;

        stateMachine.Update();
    }

    private void OnDisable()
    {
        // desactive inputs
        _inputs.Disable();
    }

    private void OnDestroy()
    {
        
    }

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PUBLIC FONCTION
    //==============================================================================================================

    /// <summary>
    /// Get Current State If State Machine
    /// </summary>
    public StateId GetCurrentState()
    {
        return stateMachine.currentState;
    }

    /// <summary>
    /// Get hit from something, like the creature.
    /// </summary>
    /// <param name="value"></param>
    public void GetHit(int value)
    {
        playerData.inGameDataValue.hp -= value;
        playerData.inGameDataValue.hp = Math.Clamp(playerData.inGameDataValue.hp, 0, 10);
        if (playerData.inGameDataValue.hp == 0) playerData.monitor.isDead = true;

        stateMachine.ChangeState(StateId.GETHIT);
    }

    /// <summary>
    /// Set State of player to idle.
    /// </summary>
    public void ChangeStateToIdle() => stateMachine.ChangeState(StateId.IDLE);

    /// <summary>
    /// Exit dodge state.
    /// </summary>
    public void StartDodge() 
    { 
        playerData.monitor.canGetHit = false; 
    }

    /// <summary>
    /// Exit dodge state.
    /// </summary>
    public void EndDodge() { stateMachine.ChangeState(StateId.IDLE); Debug.Log("end dodge"); }

    /// <summary>
    /// Attack by making a new component of the weapon to call the fonction, then destroy the Monobehaviour.
    /// </summary>
    public void Atk1()
    {
        GameObject obj_weapon = Instantiate(weapon.go_Weapon_Hit_Prefab, _Body.transform);
        Weapon _weapon = obj_weapon.GetComponent<Weapon>();
        _weapon.Atk1();

    }

    /// <summary>
    /// Attack by making a new component of the weapon to call the fonction, then destroy the Monobehaviour.
    /// </summary>
    public void Atk2()
    {
        GameObject obj_weapon = Instantiate(weapon.go_Weapon_Hit_Prefab, _Body.transform);
        Weapon _weapon = obj_weapon.GetComponent<Weapon>();
        _weapon.Atk2();

    }

    /// <summary>
    /// Remove The Ability for the player to move, until he dodge or finish healing
    /// </summary>
    public void ChangeStateToPlayerHealing()
    {
        stateMachine.ChangeState(StateId.HEALING);
    }

    /// <summary>
    /// Revive player with 4hp.
    /// </summary>
    public void Revive()
    {
        
    }

    public void Died() => stateMachine.ChangeState(StateId.DEATH);

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PRIVATE FONCTION
    //==============================================================================================================

    #region Data

    [Button]
    private void ResetPlayerDataToDefaultData()
    {
        playerData.inGameDataValue.speed = 10.0f;
        playerData.inGameDataValue.sprintSpeed = 5.333f;
        playerData.inGameDataValue.speedChangeRate = 10.0f;
        playerData.inGameDataValue.hitCooldown = 2.0f;
        playerData.inGameDataValue.hp = 10;
    }

    private void ResetPlayerData()
    {
        playerData.SetNewMonitor();
        playerData.SetNewVariables();
    }

    #endregion

    #region InputsEvent

    /// <summary>
    /// Register all inputs for the player from the Input Actions.
    /// </summary>
    private void RegisterInputEvent()
    {
        // Dodge
        _inputs.Player.Dodge.started += ctx => playerData.monitor.tryToDodge = true;
        _inputs.Player.Dodge.canceled += ctx => playerData.monitor.tryToDodge = false;
        // Attack1
        _inputs.Player.Attack1.started += ctx => playerData.monitor.tryToAtk1 = true;
        _inputs.Player.Attack1.canceled += ctx => playerData.monitor.tryToAtk1 = false;
        // Attack2
        _inputs.Player.Attack2.started += ctx => playerData.monitor.tryToAtk2 = true;
        _inputs.Player.Attack2.canceled += ctx => playerData.monitor.tryToAtk2 = false;
        // Equipment1
        _inputs.Player.Equipment1.started += ctx => _equipment1.SetOnSelected();
        // Equipment2
        _inputs.Player.Equipment2.started += ctx => _equipment2.SetOnSelected();
        // Drop
        _inputs.Player.Drop.started += ctx => _equipment1.Drop();
        _inputs.Player.Drop.started += ctx => _equipment2.Drop();
    }

    /// <summary>
    /// Gather input each frame.
    /// </summary>
    private void GatherInput()
    {
        movementInput = _inputs.Player.Movement.ReadValue<Vector2>();
        if (movementInput.magnitude < 0.1f) playerData.monitor.tryToMove = false; else playerData.monitor.tryToMove = true;

        Vector2 mousePosition = _inputs.Player.LocationLook.ReadValue<Vector2>();
        Vector3 mousePositionInWorld = _Camera.ScreenToWorldPoint(mousePosition);
        LookDirectionRelativeToTransformOfPlayer(mousePosition);
    }

    #endregion

    #region Parameters

    /// <summary>
    /// Update all parameters.
    /// </summary>
    [Button]
    private void UpdateParameters()
    {
        SetVirtualCamParameters();
    }
    /// <summary>
    /// Reset all parameters to there default values.
    /// </summary>
    [Button]
    private void ResetParameters()
    {
        UpdateParameters();
    }

    /// <summary>
    /// Set a bunch of things on the virtual cam.
    /// </summary>
    private void SetVirtualCamParameters()
    {

    }

    #endregion

    #region State Machine
    /// <summary>
    /// set all state with their fonction in the state machine
    /// </summary>
    private void SetStates()
    {
        playerData.monitor.isChangingState = true;

        // set state machine
        stateMachine = new StateMachine<Tps_PlayerController>(Instance);

        StateInit<Tps_PlayerController> stateInit = new StateInit<Tps_PlayerController>();
        stateInit.delegateEventsAtInitOfState += InitStateInit;
        stateInit.delegateEventsAtUpdateOfState += UpdateStateInit;
        stateInit.delegateEventsAtExitOfState += ExitStateInit;
        stateMachine.RegisterState(stateInit);

        StateIsntLoaded<Tps_PlayerController> stateIsntLoaded = new StateIsntLoaded<Tps_PlayerController>();
        stateIsntLoaded.delegateEventsAtInitOfState += InitStateIsntLoaded;
        stateIsntLoaded.delegateEventsAtUpdateOfState += UpdateStateIsntLoaded;
        stateIsntLoaded.delegateEventsAtExitOfState += ExitStateIsntLoaded;
        stateMachine.RegisterState(stateIsntLoaded);

        StateIdle<Tps_PlayerController> stateIdle = new StateIdle<Tps_PlayerController>();
        stateIdle.delegateEventsAtInitOfState += InitStateIdle;
        stateIdle.delegateEventsAtUpdateOfState += UpdateStateIdle;
        stateIdle.delegateEventsAtExitOfState += ExitStateIdle;
        stateMachine.RegisterState(stateIdle);

        StateDodge<Tps_PlayerController> stateDodge = new StateDodge<Tps_PlayerController>();
        stateDodge.delegateEventsAtInitOfState += InitStateDodge;
        stateDodge.delegateEventsAtUpdateOfState += UpdateStateDodge;
        stateDodge.delegateEventsAtExitOfState += ExitStateDodge;
        stateMachine.RegisterState(stateDodge);

        StateAtk1<Tps_PlayerController> stateAtk1 = new StateAtk1<Tps_PlayerController>();
        stateAtk1.delegateEventsAtInitOfState += InitStateAtk1;
        stateAtk1.delegateEventsAtUpdateOfState += UpdateStateAtk1;
        stateAtk1.delegateEventsAtExitOfState += ExitStateAtk1;
        stateMachine.RegisterState(stateAtk1);

        StateAtk2<Tps_PlayerController> stateAtk2 = new StateAtk2<Tps_PlayerController>();
        stateAtk2.delegateEventsAtInitOfState += InitStateAtk2;
        stateAtk2.delegateEventsAtUpdateOfState += UpdateStateAtk2;
        stateAtk2.delegateEventsAtExitOfState += ExitStateAtk2;
        stateMachine.RegisterState(stateAtk2);

        StateEquipement<Tps_PlayerController> stateEquipement = new StateEquipement<Tps_PlayerController>();
        stateEquipement.delegateEventsAtInitOfState += InitStateEquipement;
        stateEquipement.delegateEventsAtUpdateOfState += UpdateStateEquipement;
        stateEquipement.delegateEventsAtExitOfState += ExitStateEquipement;
        stateMachine.RegisterState(stateEquipement);

        StateHealing<Tps_PlayerController> stateHealing = new StateHealing<Tps_PlayerController>();
        stateHealing.delegateEventsAtInitOfState += InitStateHealing;
        stateHealing.delegateEventsAtUpdateOfState += UpdateStateHealing;
        stateHealing.delegateEventsAtExitOfState += ExitStateHealing;
        stateMachine.RegisterState(stateHealing);

        StateGetHit<Tps_PlayerController> stateGetHit = new StateGetHit<Tps_PlayerController>();
        stateGetHit.delegateEventsAtInitOfState += InitStateGetHit;
        stateGetHit.delegateEventsAtUpdateOfState += UpdateStateGetHit;
        stateGetHit.delegateEventsAtExitOfState += ExitStateGetHit;
        stateMachine.RegisterState(stateGetHit);

        StatePaused<Tps_PlayerController> statePaused = new StatePaused<Tps_PlayerController>();
        stateIdle.delegateEventsAtInitOfState += InitStatePaused;
        stateIdle.delegateEventsAtUpdateOfState += UpdateStatePaused;
        stateIdle.delegateEventsAtExitOfState += ExitStatePaused;
        stateMachine.RegisterState(stateIdle);

        StateDead<Tps_PlayerController> stateDeath = new StateDead<Tps_PlayerController>();
        stateDeath.delegateEventsAtInitOfState += InitDeath;
        stateDeath.delegateEventsAtUpdateOfState += UpdateDeath;
        stateDeath.delegateEventsAtExitOfState += ExitDeath;
        stateMachine.RegisterState(stateDeath);

        stateMachine.StartingState(StateId.INIT);
    }
    /// <summary>
    /// Check all state.
    /// </summary>
    private void CheckStates()
    {
        // return if the state machine is currently changing state.
        if (playerData.monitor.isChangingState || !playerData.monitor.isValid) return;

        // return while player isn't corretly initialized.
        if (!playerData.monitor.isInit) { SetStates(); return; }
    }

    #region States Actions

    #region Init
    private void InitStateInit()
    {
        RegisterInputEvent();

        AssignAnimationIDs();

        playerData.monitor.isInit = true;
        playerData.monitor.isActive = true;
        playerData.monitor.isChangingState = false;

        stateMachine.ChangeState(StateId.IDLE);
    }
    private void UpdateStateInit()
    {

    }
    private void ExitStateInit()
    {

    }
    #endregion
    #region Isn't Loaded
    private void InitStateIsntLoaded()
    {

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateIsntLoaded()
    {

    }
    private void ExitStateIsntLoaded()
    {

    }
    #endregion
    #region Idle
    private void InitStateIdle()
    {
        playerData.monitor.canMove = true;
        playerData.monitor.canDodge = true;
        playerData.monitor.canAtk1 = true;
        playerData.monitor.canAtk2 = true;
        playerData.monitor.canGetHit = true;

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateIdle()
    {
        MoveFlashlight(); FlipBody();
        // move the player.
        Move(HunterMoveType.NORMAL);

        if(playerData.monitor.isAtk1) stateMachine.ChangeState(StateId.ATK1);
        if (playerData.monitor.isAtk2) stateMachine.ChangeState(StateId.ATK2);
    }
    private void ExitStateIdle()
    {

    }
    #endregion
    #region Dodge
    private void InitStateDodge()
    {
        playerData.variables.speed = 0.0f;
        _Animator.SetFloat(_animIDSpeed, 0.0f);
        _Animator.SetTrigger(_animIDDodge);

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateDodge()
    {
        MoveFlashlight();
    }
    private void ExitStateDodge()
    {
        playerData.monitor.isChangingState = true;

        playerData.monitor.canGetHit = true;
    }
    #endregion
    #region Atk1
    private void InitStateAtk1()
    {
        _Animator.SetFloat(_animIDSpeed, 0.0f);
        _Animator.SetTrigger(_animIDAtk1);

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateAtk1()
    {
        MoveFlashlight(); 
        FlipBody();
        Move(HunterMoveType.STOP);
        if (playerData.monitor.isDodging) stateMachine.ChangeState(StateId.DODGE);
    }
    private void ExitStateAtk1()
    {
        playerData.monitor.isChangingState = true;
    }
    #endregion
    #region Atk2
    private void InitStateAtk2()
    {
        _Animator.SetFloat(_animIDSpeed, 0.0f);
        _Animator.SetTrigger(_animIDAtk2);

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateAtk2()
    {
        MoveFlashlight();
        FlipBody();
        Move(HunterMoveType.STOP);
    }
    private void ExitStateAtk2()
    {
        playerData.monitor.isChangingState = true;
    }
    #endregion
    #region Equipement
    private void InitStateEquipement()
    {
        _Animator.SetFloat(_animIDSpeed, 0.0f);
        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateEquipement()
    {
        MoveFlashlight();
        FlipBody();
        Move(HunterMoveType.NORMAL);
    }
    private void ExitStateEquipement()
    {

    }
    #endregion
    #region Healing
    private void InitStateHealing()
    {
        _Animator.SetTrigger(_animIDHealing);

        currentMedkit = new Medkit_Script (this, 1, 10, 5);

        currentMedkit.OnUse();

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateHealing()
    {
        //MoveFlashlight();
        //FlipBody();
        //Move(HunterMoveType.NORMAL);
    }
    private void ExitStateHealing()
    {
        playerData.monitor.isChangingState = true;

        //Check If For Assuring The Player doesn't use 2 medkit insteed of one
        if (!currentMedkit.DidPlayerFinishUsingThisMedkit())
        {
            currentMedkit.SetPlayerCanUseMedkit(false);
        }
    }
    #endregion
    #region Get Hit
    private void InitStateGetHit()
    {
        playerData.variables.speed = 0.0f;
        _Animator.SetFloat(_animIDSpeed, 0.0f);
        _Animator.SetTrigger(_animIDGetHit);
        StartCoroutine(CooldownHit());

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateGetHit()
    {
        MoveFlashlight();
        FlipBody();

        if (playerData.monitor.isDead) stateMachine.ChangeState(StateId.DEATH);
        if (!playerData.monitor.isStun && !playerData.monitor.isDead) stateMachine.ChangeState(StateId.IDLE);
    }
    private void ExitStateGetHit()
    {
        playerData.monitor.isChangingState = true;
    }
    #endregion
    #region Paused
    private void InitStatePaused()
    {

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStatePaused()
    {

    }
    private void ExitStatePaused()
    {

    }
    #endregion
    #region Death
    private void InitDeath()
    {
        _Animator.SetTrigger(_animIDDeath);

        playerData.monitor.isChangingState = false;
    }
    private void UpdateDeath()
    {
        if (playerData.monitor.getRevive) IsGettingRevived();
        else
        {
            revivingTimer = 0;
            _Animator.SetBool(_animIDRevive, false);
        }
    }
    private void ExitDeath()
    {

    }
    #endregion

    #endregion

    #endregion

    #region Movement

    /// <summary>
    /// Set ID of animation to there value.
    /// </summary>
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDDodge = Animator.StringToHash("Dodge");
        _animIDAtk1 = Animator.StringToHash("Atk1");
        _animIDAtk2 = Animator.StringToHash("Atk2");
        _animIDHealing = Animator.StringToHash("Healing");
        _animIDGetHit = Animator.StringToHash("GetHit");
        _animIDDeath = Animator.StringToHash("Death");
        _animIDRevive = Animator.StringToHash("Revive");
    }

    /// <summary>
    /// Movement of player.
    /// </summary>
    private void Move(HunterMoveType hunterMoveType)
    {
        if (!playerData.monitor.canMove) {
            playerData.variables.speed = 0.0f;
            return; }

        // set target speed based on move speed, sprint speed and if sprint is pressed
        // float targetSpeed = playerData.monitor.isSprinting ? playerData.inGameDataValue.sprintSpeed : playerData.inGameDataValue.speed;
        float targetSpeed = playerData.inGameDataValue.speed;

        // normalise input direction
        Vector3 inputDirection = new Vector3(movementInput.x, 0.0f, movementInput.y).normalized;

        float inputMagnitude = movementInput.magnitude;

        // if there is no input, set the target speed to 0
        if (!playerData.monitor.tryToMove) targetSpeed = 0.0f;

        // wich type of movement it is.
        switch (hunterMoveType)
        {
            case HunterMoveType.NORMAL:
                break;
            case HunterMoveType.STOP:
                targetSpeed = 0.0f;
                break;
            default:
                break;
        }

        // acc / dec
        float lerpedTargetSpeed = Mathf.Lerp(playerData.variables.speed, targetSpeed, playerData.inGameDataValue.speedChangeRate * Time.deltaTime);

        playerData.variables.speed = lerpedTargetSpeed;


            _animSpeedBlend = Mathf.Lerp(_animSpeedBlend, lerpedTargetSpeed / targetSpeed, Time.deltaTime * playerData.inGameDataValue.speedChangeRate);
        if (_animSpeedBlend < 0.01f) _animSpeedBlend = 0f;


        if (playerData.monitor.tryToMove)
        {
            // if there is a move input rotate player when the player is moving
            _movementTargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                _Camera.transform.eulerAngles.y;
            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, _movementTargetRotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _movementTargetRotation, 0.0f) * Vector3.forward;
        Vector3 newPosition = transform.position + targetDirection * Time.deltaTime * playerData.variables.speed;

        // look if there is space to move
        NavMeshHit hit;
        bool isValid = NavMesh.SamplePosition(newPosition, out hit, .3f, NavMesh.AllAreas);

        // if there is space to move and there is enough movement
        if (isValid && (transform.position - hit.position).magnitude >= .02f)
            transform.position = hit.position; // movement

        // Set camera target to it's position.
        _targetCamera.transform.position = transform.position + Vector3.up * 1.65f;

        // Set sprite pos to player pos.
        _Body.transform.position = transform.position + Vector3.up * 0.2f;

        // Set flashlight pos to player pos
        _flashlightRoot.position = transform.position + Vector3.up * 0.01f;

        // update animator
        _Animator.SetFloat(_animIDSpeed, _animSpeedBlend);
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CooldownHit()
    {
        playerData.monitor.canGetHit = false;
        yield return new WaitForSeconds(playerData.inGameDataValue.hitCooldown);
        playerData.monitor.canGetHit = true;
    }

    /// <summary>
    /// look for the direction from player to you objective.
    /// </summary>
    private void LookDirectionRelativeToTransformOfPlayer(Vector2 objectif)
    {
        // target player to it's screen position.
        Vector3 playerPosInViewport = _Camera.WorldToScreenPoint(_targetCamera.transform.position);

        // direction of the vector from player to mouse.
        Vector2 _direction = new Vector2(playerPosInViewport.x - objectif.x, playerPosInViewport.y - objectif.y);

        // calculate Y rotation from the direction.
        _lookTargetRotation = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg + 90;
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveFlashlight()
    {
        Vector3 _direction = new Vector3(0, _lookTargetRotation, 0);
        Vector3 _lerpedDirection = Vector3.Lerp(_flashlightRoot.rotation.eulerAngles, _direction, 1f);
        _flashlightRoot.localRotation = Quaternion.Euler(_lerpedDirection);
    }

    private void FlipBody()
    {
        if (_lookTargetRotation > 90)
        {
            _Body.transform.localScale = new Vector3(-1, 1, 1);
        }

        else
        {
            _Body.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void IsGettingRevived()
    {
        _Animator.SetBool(_animIDRevive, true);
        revivingTimer += Time.deltaTime;
        if (revivingTimer > playerData.inGameDataValue.reviveTime) stateMachine.ChangeState(StateId.IDLE);
    }


    #endregion
    //==============================================================================================================
}