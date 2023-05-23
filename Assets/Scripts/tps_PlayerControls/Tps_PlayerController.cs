using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AkarisuMD.Player;
using NaughtyAttributes;

/// <summary>
/// tps player using starter pack model.
/// /!\ made with a singleton /!\
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Tps_PlayerController : Singleton<Tps_PlayerController>
{


    //==============================================================================================================
    #region PUBLIC
    //==============================================================================================================

    /// <summary>
    /// Scritable singleton for storing player data.
    /// </summary>
    public ScS_PlayerData playerData;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region REFERENCE
    //==============================================================================================================
    [SerializeField] private CharacterController _CharacterController;
    [SerializeField] private GameObject _Body;
    [SerializeField] private Animator _Animator;
    [SerializeField] private Camera _Camera;
    [SerializeField] private GameObject _targetCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PRIVATE
    //==============================================================================================================
    private Tps_Player_Inputs _inputs;

    [Space(5), Header("Machine state")]
    [SerializeField] private StateMachine<Tps_PlayerController> stateMachine;

    //input
    private Vector2 movementInput;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDDodge;
    private int _animIDAtk1;
    private int _animIDAtk2;
    private int _animIDGetHit;
    private int _animIDDeath;
    private int _animIDRevive;

    private float _animIDSpeedBlend;

    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private const float Rad2Deg = 57.29578f;

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region MONOBEHAVIOUR
    //==============================================================================================================

    private void Awake()
    {
        // create inputs.
        _inputs = new();
    }

    private void OnEnable()
    {
        // active inputs
        _inputs.Enable();
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
    /// Exit dodge state.
    /// </summary>
    public void StartDodge() => playerData.monitor.canGetHit = false;
    /// <summary>
    /// Exit dodge state.
    /// </summary>
    public void EndDodge() => stateMachine.ChangeState(StateId.IDLE);
    /// <summary>
    /// deal damage from attaque 1.
    /// </summary>
    public void ATK1()
    {

    }
    /// <summary>
    /// deal damage from attaque 2.
    /// </summary>
    public void ATK2()
    {

    }
    /// <summary>
    /// Revive player with 4hp.
    /// </summary>
    public void Revive()
    {

    }

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PRIVATE FONCTION
    //==============================================================================================================

    #region Data

    [Button]
    private void ResetPlayerDataToDefaultData()
    {
        playerData.inGameDataValue.speed = 2.0f;
        playerData.inGameDataValue.sprintSpeed = 5.333f;
        playerData.inGameDataValue.speedChangeRate = 10.0f;
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
    }

    /// <summary>
    /// Gather input each frame.
    /// </summary>
    private void GatherInput()
    {
        movementInput = _inputs.Player.Movement.ReadValue<Vector2>();
        if (movementInput.magnitude < 0.1f) playerData.monitor.tryToMove = false; else playerData.monitor.tryToMove = true;
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
        // gather all input of player.
        GatherInput();

        // move the player.
        Move();
    }
    private void ExitStateIdle()
    {

    }
    #endregion
    #region Dodge
    private void InitStateDodge()
    {
        _Animator.SetTrigger(_animIDDodge);

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateDodge()
    {

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


        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateAtk1()
    {

    }
    private void ExitStateAtk1()
    {

    }
    #endregion
    #region Atk2
    private void InitStateAtk2()
    {


        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateAtk2()
    {

    }
    private void ExitStateAtk2()
    {

    }
    #endregion
    #region Equipement
    private void InitStateEquipement()
    {


        playerData.monitor.isChangingState = false;
    }
    private void UpdateStateEquipement()
    {

    }
    private void ExitStateEquipement()
    {

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

        playerData.monitor.isChangingState = false;
    }
    private void UpdateDeath()
    {

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
        _animIDGetHit = Animator.StringToHash("GetHit");
        _animIDDeath = Animator.StringToHash("Death");
        _animIDRevive = Animator.StringToHash("Revive");
    }

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        // float targetSpeed = playerData.monitor.isSprinting ? playerData.inGameDataValue.sprintSpeed : playerData.inGameDataValue.speed;
        float targetSpeed = playerData.inGameDataValue.speed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        float inputMagnitude = movementInput.magnitude;
        // if there is no input, set the target speed to 0
        if (!playerData.monitor.tryToMove) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_CharacterController.velocity.x, 0.0f, _CharacterController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            playerData.variables.speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * playerData.inGameDataValue.speedChangeRate);

            // round speed to 3 decimal places
            playerData.variables.speed = Mathf.Round(playerData.variables.speed * 1000f) / 1000f;
        }
        else
        {
            playerData.variables.speed = targetSpeed;
        }

        _animIDSpeedBlend = Mathf.Lerp(_animIDSpeedBlend, targetSpeed, Time.deltaTime * playerData.inGameDataValue.speedChangeRate);
        if (_animIDSpeedBlend < 0.01f) _animIDSpeedBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(movementInput.x, 0.0f, movementInput.y).normalized;

        if (playerData.monitor.tryToMove)
        {
            // if there is a move input rotate player when the player is moving
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                _Camera.transform.eulerAngles.y;
            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _CharacterController.Move(targetDirection * playerData.variables.speed * Time.deltaTime +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // Set camera target to it's position.
        _targetCamera.transform.position = transform.position + Vector3.up * 1.65f;

        _Body.transform.position = transform.position;

        // update animator
        _Animator.SetFloat(_animIDSpeed, _animIDSpeedBlend);
    }


    private void Attack1()
    {

    }

    private void Attack2()
    {

    }

    private void IsStun()
    {

    }

    #endregion

    // Audio

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            // audio
        }
    }

    #endregion
    //==============================================================================================================
}