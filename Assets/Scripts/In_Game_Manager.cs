using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class In_Game_Manager : Singleton<In_Game_Manager>
{
    //========
    //VARIABLES
    //========

    [Header("Ref GameObject")]
    [SerializeField] private GameObject monsterGameObject;
    [SerializeField] private List<GameObject> soldiersGameObject = new();

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera startCamera;

    private Dictionary<CinemachineVirtualCamera, PlayerInput> soldiersComponent = new();
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
                    soldier.GetComponentInChildren<PlayerInput>()
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
        if (playerId == 0)
        {
            SwitchCamera(monsterComponent.Item1);
            ActivateInput(monsterComponent.Item2);
        }
        else
        {
            //We remove 1 because we want to take the first element of the list
            //Soldier 1 have a player id of 1 but we want him to take the first element of the list so 0
            CinemachineVirtualCamera soldierCamera = soldiersComponent.ElementAt(playerId - 1).Key;
            PlayerInput soldierInput = soldiersComponent.ElementAt(playerId - 1).Value;

            SwitchCamera(soldierCamera);
            ActivateInput(soldierInput);
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
    /// Activate The Player Input
    /// </summary>
    /// <param name="playerInput"></param>
    private void ActivateInput(PlayerInput playerInput)
    {
        playerInput.enabled = true;
        Debug.Log("Activate Input");
    }
}
