using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Cinemachine;
using UnityEngine.InputSystem;
using System;

public class In_Game_Manager : Singleton<In_Game_Manager>
{
    //========
    //VARIABLES
    //========

    /// <summary>
    /// When the game is Starting with the player id of the actual player
    /// </summary>

    [Header("Ref GameObject")]
    [SerializeField] private GameObject monsterGameObject;
    [SerializeField] private List<GameObject> soldiersGameObject = new();
    [SerializeField] private GameObject hunterUI;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera startCamera;

    private Dictionary<CinemachineVirtualCamera, Tps_PlayerController> soldiersComponent = new();
    private (CinemachineVirtualCamera, PlayerInput) monsterComponent;


    //========
    //MONOBEHAVIOUR
    //========
    private void Awake()
    {
        //Get all the component
        if(monsterGameObject != null)
        {
            monsterComponent.Item1 = monsterGameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            monsterComponent.Item2 = monsterGameObject.GetComponentInChildren<PlayerInput>();
        }

        if(soldiersGameObject.Count > 0)
        {
            foreach(GameObject soldier in soldiersGameObject)
            {
                soldiersComponent.Add(
                    soldier.GetComponentInChildren<CinemachineVirtualCamera>(),
                    soldier.GetComponentInChildren<Tps_PlayerController>()
                );
            }
        }
    }

    //========
    //FONCTION
    //========


    /// <summary>
    /// Change The Camera And Activate Input 
    /// </summary>
    /// <param name="playerId"></param>
    public void GiveInputAndCameraToPlayer(int playerId)
    {
        GameObject newPlayer = null;
        if (playerId == 0)
        {
            SwitchCamera(monsterComponent.Item1);
            ActivateInputMonster(monsterComponent.Item2);

            newPlayer = monsterGameObject;

            UIGlobal_Manager.Instance.SwitchUIState(UIState.Monster);

            foreach (Tps_PlayerController tps_PlayerController in soldiersComponent.Values)
            {
                Destroy(tps_PlayerController);
            }

            MiniMapManager.Instance.canvas.SetActive(true);
            MiniMapManager.Instance.SetForMonster();
        }
        else
        {
            //We remove 1 because we want to take the first element of the list
            //Soldier 1 have a player id of 1 but we want him to take the first element of the list so 0
            CinemachineVirtualCamera soldierCamera = soldiersComponent.ElementAt(playerId - 1).Key;
            Tps_PlayerController soldierScript = soldiersComponent.ElementAt(playerId - 1).Value;

            SwitchCamera(soldierCamera);
            ActivateInputSoldier(soldierScript);

            newPlayer = soldiersGameObject[playerId - 1];
            soldiersComponent.Remove(soldierCamera);

            UIGlobal_Manager.Instance.SwitchUIState(UIState.Soldier);

            newPlayer.GetComponentInChildren<HunterHitCollider>().SetPlayerIdServerRpc(playerId);
            HealthBarManager.Instance.IndexOwner = playerId;

            foreach (Tps_PlayerController tps_PlayerController in soldiersComponent.Values)
            {
                Destroy(tps_PlayerController);
            }

            MiniMapManager.Instance.canvas.SetActive(true);
            MiniMapManager.Instance.SetForHunter();
        }
    }
    public GameObject GetPlayerViaId(int playerId)
    {
        if(playerId == 0)
        {
            return monsterGameObject;
        }
        else
        {
            return soldiersGameObject[playerId - 1];
        }
    }
    /// <summary>
    /// Deactive the other camera to switch on this camera
    /// </summary>
    /// <param name="newCamera"></param>
    private void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        //Deactivate the other camera
        startCamera.Priority = 0;
        foreach (CinemachineVirtualCamera virtualCamera in soldiersComponent.Keys)
        {
            virtualCamera.Priority = 0;
        }
        monsterComponent.Item1.Priority = 0;

        newCamera.Priority = 10;

        Debug.Log("Switch to" + newCamera.name + "camera");
    }
    /// <summary>
    /// Activate The Monster Input
    /// </summary>
    /// <param name="playerInput"></param>
    private void ActivateInputMonster(PlayerInput playerInput)
    {
        playerInput.enabled = true;
        Debug.Log("Activate Input");
    }
    /// <summary>
    /// Activate The Soldier Input
    /// </summary>
    /// <param name="soldier_Script"></param>
    private void ActivateInputSoldier(Tps_PlayerController soldiertps_Script)
    {
        soldiertps_Script.enabled = true;
        soldiertps_Script.Inputs.Enable();
        //soldiertps_Script.isOwner();
        Debug.Log("Input Soldier Activated");
    }
}
