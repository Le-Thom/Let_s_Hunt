using Akarisu;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class CloudManager : Singleton<CloudManager>
{

    //==============================================================================================================
    #region PUBLIC
    //==============================================================================================================


    public static List<string> playerIdsLocalCache { get; private set; }

    //=================**
    // SETTINGS TO SAVE
    //=================**

    /// <summary>
    /// Parameters settings of game.
    /// </summary>
    public GameSettingsParameters GameSettingsParameters;


    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region REFERENCE
    //==============================================================================================================

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region PRIVATE
    //==============================================================================================================

    #endregion
    //==============================================================================================================

    //==============================================================================================================
    #region MONOBEHAVIOUR
    //==============================================================================================================

    private void Awake()
    {
        InitializeCloudAsync();
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
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

    #endregion
    //==============================================================================================================


    /// <summary>
    /// Initialize the Core SDK using await UnityServices.InitializeAsync().
    /// Sign in using the Authentication SDK.
    /// </summary>
    private async void InitializeCloudAsync()
    {
        await UnityServices.InitializeAsync();
        if (this == null) return;

        Debug.Log("Services Initialized.");

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("Signing in...");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if (this == null) return;
        }

        AddNewPlayerId(AuthenticationService.Instance.PlayerId);
        Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");
    }
    public static void AddNewPlayerId(string playerId)
    {
        if (!playerIdsLocalCache.Contains(playerId))
        {
            playerIdsLocalCache.Add(playerId);
        };
    }

    /// <summary>
    /// Save settings in cloud.
    /// </summary>
    private void SaveSettings() => SaveService.SaveSettings(GameSettingsParameters);

}
