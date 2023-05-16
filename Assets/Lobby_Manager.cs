using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System;

public class Lobby_Manager : NetworkBehaviour
{
    //========
    //VARIABLES
    //========
    [SerializeField] private GameObject lobbyMenu_UI;
    [SerializeField] private TextMeshProUGUI streamerName;
    private NetworkVariable<int> numberOfPlayerInTheRoom = new NetworkVariable<int>(0);
    [SerializeField] private List<SoldierLobby> soldierLobbies = new List<SoldierLobby>();
    private int playerLimit => soldierLobbies.Count;

    //========
    //MONOBEHAVIOUR
    //========

    private void Update()
    {
        
    }
    private void OnApplicationQuit()
    {
        NetworkManager.Shutdown();
    }

    //========
    //FONCTION
    //========

    public void GetInLobby()
    {
        lobbyMenu_UI.SetActive(true);
        if (NetworkManager.Singleton.IsHost)
        {
            streamerName.text = GetTwitchData_Script.Instance.GetChannelName();
            numberOfPlayerInTheRoom.Value = 0;
            return;
        }
        else
        {
            numberOfPlayerInTheRoom.Value++;
        }
    }
    public void StartTheGame()
    {
        if (IsAllSoliderReady() && NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Starting The Game");
            //Make All The UI Disapear

            //Start The Game

        }
    }
    private bool IsAllSoliderReady()
    {
        SoldierLobby[] allSoldier = GameObject.FindObjectsOfType<SoldierLobby>();
        bool value = true;
        foreach(SoldierLobby soldier in allSoldier)
        {
            if (!soldier.isTheSoldierReady) value = false;
        }
        return value;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddingPlayerServerRpc()
    {
        AddingPlayerClientRpc();
    }
    [ClientRpc]
    private void AddingPlayerClientRpc()
    {
        soldierLobbies[numberOfPlayerInTheRoom.Value].WhenSoliderJoin();
        print("Testing");
    }
    private bool IsRoomFull()
    {
        if (numberOfPlayerInTheRoom.Value >= playerLimit)
        {
            return true;
        }
        else return false;
    }
}
