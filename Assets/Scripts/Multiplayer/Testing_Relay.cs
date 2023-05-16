using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using NaughtyAttributes;

public class Testing_Relay : MonoBehaviour
{
    //=======
    //MONOBEHAVIOR
    //=======
    [SerializeField] private GetTwitchData_Script twitch_Script;
    [SerializeField] private Lobby_Manager lobby_Manager;

    //=======
    //MONOBEHAVIOR
    //=======
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => { Debug.Log("Signed In " + AuthenticationService.Instance.PlayerId); };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    private void OnEnable()
    {
        twitch_Script.onConnectionSuccess.AddListener(ConnectToRelay);
    }
    private void OnDisable()
    {
        twitch_Script.onConnectionSuccess.RemoveListener(ConnectToRelay);
    }
    //=======
    //FONCTION
    //=======
    [Button]
    private async void ConnectToRelay()
    {
        try 
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("join Code: " + joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4, 
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData);

            NetworkManager.Singleton.StartHost();

            lobby_Manager.GetInLobby();

        } catch (RelayServiceException error) 
        {
            Debug.LogError(error);
        }
    }
    public async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            Debug.Log("Joining Relay with " + joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4, 
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
                );

            NetworkManager.Singleton.StartClient();

            lobby_Manager.GetInLobby();
        }
        catch (RelayServiceException error)
        {
            Debug.LogError(error);
        }
    }
}
