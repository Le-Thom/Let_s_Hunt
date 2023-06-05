using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class SpotlightManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> spotlights = new();

    [ClientRpc]
    public void CallSpotlightOnClientRpc() => CallSpotlightOn();
    public async void CallSpotlightOn()
    {
        foreach (var spotlight in spotlights)
        {
            spotlight.SetActive(true);
        }

        await Task.Delay(30000);


        foreach (var spotlight in spotlights)
        {
            spotlight.SetActive(false);
        }
    }
}
