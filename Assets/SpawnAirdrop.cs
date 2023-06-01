using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnAirdrop : NetworkBehaviour
{
    [SerializeField] private GameObject prefabAirdrop;

    [Button, ServerRpc(RequireOwnership = false)]
    public void Spawn_AirdropServerRpc()
    {
        GameObject _obj = Instantiate(prefabAirdrop);
        _obj.GetComponent<NetworkObject>().Spawn(true);
    }
}
