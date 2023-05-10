using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player")]
public class ScS_PlayerData : ScriptableObject
{
    #region singleton
    private static ScS_PlayerData instance;
    public static ScS_PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<ScS_PlayerData>("PlayerData");
            }
            return instance;
        }
    }
    #endregion

    /// <summary>
    /// All state active and used of the controller.
    /// </summary>
    public PlayerMonitor monitor;
    public void SetNewMonitor() { monitor = new(); }
    /// <summary>
    /// struct to control Player.
    /// </summary>
    [Serializable]
    public struct PlayerMonitor
    {
        /// <summary>
        /// can Player start working.
        /// </summary>
        public bool isValid;
        /// <summary>
        /// State of player is changing.
        /// </summary>
        public bool isChangingState;
        /// <summary>
        /// is Player initialized.
        /// </summary>
        public bool isInit;
        /// <summary>
        /// is Player correctly working.
        /// </summary>
        public bool isActive;

        // check if player want to move and can move.
        public bool tryToMove;
        public bool canMove;
        public bool isMoving => tryToMove && canMove;

        // check if player want to dodge and can dodge.
        public bool tryToDodge;
        public bool canDodge;
        public bool isDodging => tryToDodge && canDodge;

        // check if player want to Attack1 and can Attack1.
        public bool tryToAttack1;
        public bool canAttack1;
        public bool isAttack1 => tryToAttack1 && canAttack1;

        // check if player want to Attack2 and can Attack2.
        public bool tryToAttack2;
        public bool canAttack2;
        public bool isAttack2 => tryToAttack2 && canAttack2;

        /// <summary>
        /// Used for hitCooldown to not get directly kill easily.
        /// </summary>
        public bool canGetHit;

        /// <summary>
        /// Is player stun (if true the can't do any action, if hit, return to false)
        /// </summary>
        public bool isStun;

        public bool canHeal;
        public bool canGetHealed;

        /// <summary>
        /// is Player ded. obviously.
        /// </summary>
        public bool isDead;
    }

    /// <summary>
    /// All state active and used of the controller.
    /// </summary>
    public PlayerVariables variables;
    public void SetNewVariables() { variables = new(); }
    /// <summary>
    /// struct to control Player.
    /// </summary>
    [Serializable]
    public struct PlayerVariables
    {
        public Vector3 playerPosition;
        public Vector3 playerRotation;

        public float speed;
    }

    /// <summary>
    /// struct to Data of the controller.
    /// </summary>
    public InGamePlayerGameDataValue inGameDataValue;
    public void SetNewInGameDataValue() { inGameDataValue = new(); }
    /// <summary>
    /// struct to Data of the controller.
    /// </summary>
    [Serializable]
    public struct InGamePlayerGameDataValue
    {
        [Space(10)]
        [Tooltip("Move speed of the character in m/s (Default = 2.0f)")]
        public float speed;

        [Tooltip("Sprint speed of the character in m/s (Default = 5.335f)")]
        public float sprintSpeed;

        [Tooltip("Acceleration and deceleration (Default = 10.0f)")]
        public float speedChangeRate;

        [Tooltip("The height the player can jump (Default = 1.2f)")]
        public float jumpHeight;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f (Default = -15.0f)")]
        public float gravity;
    }

    /// <summary>
    /// Clear no importante data.
    /// </summary>
    public void ClearInGameData() { }

    // ==================================================
    // need cloud save here

    /// <summary>
    /// Persitant Value of the player.
    /// </summary>
    public PersistentPlayerGameDataValue persistentPlayerGameData;
    public void SetNewPersistentPlayerGameData() { persistentPlayerGameData = new(); }
    /// <summary>
    /// Persitant Value of the player.
    /// </summary>
    [Serializable]
    public struct PersistentPlayerGameDataValue
    {
        public float globalTimeMove;
    }

    /// <summary>
    /// WARNING, clear data definitly !!!
    /// </summary>
    public void ClearPersistantData() { }
}
