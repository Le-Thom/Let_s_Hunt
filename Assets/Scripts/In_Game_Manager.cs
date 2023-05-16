using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class In_Game_Manager : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera startCamera;
    [SerializeField] private List<CinemachineVirtualCamera> solidersCamera = new();
    [SerializeField] private CinemachineVirtualCamera monsterCamera;

    [Header("Input")]
    [SerializeField] private List<PlayerInput> solidersInput = new();
    [SerializeField] private PlayerInput monsterInput;
    //========
    //MONOBEHAVIOUR
    //========

    //========
    //FONCTION
    //========


    /// <summary>
    /// Change The Camera And Activate Input 
    /// </summary>
    /// <param name="playerId"></param>
    public void StartTheGame(int playerId)
    {
        if (playerId == 0)
        {
            SwitchCamera(monsterCamera);
            ActivateInput(monsterInput);
        }
        else
        {
            //We remove 1 because we want to take the first element of the list
            //Soldier 1 have a player id of 1 but we want him to take the first element of the list so 0
            SwitchCamera(solidersCamera[playerId - 1]);
            ActivateInput(solidersInput[playerId - 1]);
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
        foreach(CinemachineVirtualCamera virtualCamera in solidersCamera)
        {
            virtualCamera.Priority = 0;
        }
        monsterCamera.Priority = 0;

        newCamera.Priority = 10;
    }
    /// <summary>
    /// Activate The Player Input
    /// </summary>
    /// <param name="playerInput"></param>
    private void ActivateInput(PlayerInput playerInput)
    {
        playerInput.enabled = true;
    }
}
