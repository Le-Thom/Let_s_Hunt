using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [Header("Settings")]
    [SerializeField] private int maxNumberOfPlayer = 5;

    [Header("Info")]
    [SerializeField] private string currentJoinCode;

    [Header("Event Create Room Streamer")]
    public UnityEvent onCreateRoomSuccess;
    public UnityEvent onCreateRoomFailed;

    [Header("Event Join Soldiers")]
    public UnityEvent onJoinRoomSuccess;
    public UnityEvent onJoinRoomFailed;

    //=======
    //MONOBEHAVIOR
    //=======
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => { Debug.Log("Signed In " + AuthenticationService.Instance.PlayerId); };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    //=======
    //FONCTION
    //=======
    [Button]
    public async void ConnectToRelay()
    {
        try 
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxNumberOfPlayer);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("join Code: " + joinCode);

            currentJoinCode = joinCode;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4, 
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData);

            NetworkManager.Singleton.StartHost();

            onCreateRoomSuccess?.Invoke();

        } catch (RelayServiceException error) 
        {
            Debug.LogError(error);

            onCreateRoomFailed?.Invoke();
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

            onJoinRoomSuccess?.Invoke();
        }
        catch (RelayServiceException error)
        {
            Debug.LogError(error);
            onJoinRoomFailed?.Invoke();
        }
    }
    public string GetJoinCode()
    {
        return currentJoinCode;
    }
}
