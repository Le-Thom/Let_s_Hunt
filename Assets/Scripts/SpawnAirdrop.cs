using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnAirdrop : NetworkBehaviour
{
    [SerializeField] private GameObject prefabAirdrop;

    [Button, ServerRpc(RequireOwnership = true)]
    public void Spawn_AirdropServerRpc()
    {
        GameObject _obj = Instantiate(prefabAirdrop, transform.position + Vector3.up * 15, Quaternion.Euler(0,0,0));
        _obj.GetComponent<NetworkObject>().Spawn(true);
    }
}
