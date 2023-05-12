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
[RequireComponent(typeof(Animator))]
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
    private int _animIDMotionSpeed;

    private float _animationBlend;

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
        _inputs.Player.Attack1.started += ctx => playerData.monitor.tryToAttack1 = true;
        _inputs.Player.Attack1.canceled += ctx => playerData.monitor.tryToAttack1 = false;
        // Attack2
        _inputs.Player.Attack2.started += ctx => playerData.monitor.tryToAttack2 = true;
        _inputs.Player.Attack2.canceled += ctx => playerData.monitor.tryToAttack2 = false;
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

        StatePerforming<Tps_PlayerController> statePerforming = new StatePerforming<Tps_PlayerController>();
        statePerforming.delegateEventsAtInitOfState += InitStatePerforming;
        statePerforming.delegateEventsAtUpdateOfState += UpdateStatePerforming;
        statePerforming.delegateEventsAtExitOfState += ExitStatePerforming;
        stateMachine.RegisterState(statePerforming);

        StatePaused<Tps_PlayerController> statePaused = new StatePaused<Tps_PlayerController>();
        statePaused.delegateEventsAtInitOfState += InitStatePaused;
        statePaused.delegateEventsAtUpdateOfState += UpdateStatePaused;
        statePaused.delegateEventsAtExitOfState += ExitStatePaused;
        stateMachine.RegisterState(statePaused);

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

        if (playerData.monitor.isActive && stateMachine.currentState != StateId.PERFORMING) stateMachine.ChangeState(StateId.PERFORMING);

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
    #region Performing
    private void InitStatePerforming()
    {
        Debug.Log("Player Enter playing state");

        playerData.monitor.canMove = true;
        playerData.monitor.canDodge = true;

        playerData.monitor.isChangingState = false;
    }
    private void UpdateStatePerforming()
    {
        // gather all input of player.
        GatherInput();

        // move the player.
        Move();
    }
    private void ExitStatePerforming()
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

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDDodge = Animator.StringToHash("Dodge");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
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

        // _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * playerData.inGameDataValue.speedChangeRate);
        // if (_animationBlend < 0.01f) _animationBlend = 0f;

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

        // update animator
        // _Animator.SetFloat(_animIDSpeed, _animationBlend);
        // _Animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }

    private void Dodge()
    {

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