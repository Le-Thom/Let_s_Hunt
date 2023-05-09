using Akarisu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class ScS_GameManager : ScriptableObject
{
    #region Singleton
    // can't use the singleton script for this so we need to make it manually.
    private static ScS_GameManager instance;
    public static ScS_GameManager Instance
    {
        get
        {
            instance = instance ?? Resources.Load<ScS_GameManager>("ManagerSingleton/GameData");
            Debug.Log(instance);
            return instance;
        }
    }
    #endregion

    /// <summary>
    /// Parameters settings of game.
    /// </summary>
    public GameSettingsParameters gameSettingsParameters;
}
