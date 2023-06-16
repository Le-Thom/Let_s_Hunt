using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnAirdrop : NetworkBehaviour
{
    [SerializeField] private GameObject prefabAirdrop1, prefabAirdrop2;

    [ServerRpc(RequireOwnership = true)]
    public void Spawn_AirdropServerRpc(int index)
    {
        GameObject _obj = null;
        if (index == 1)
            _obj = Instantiate(prefabAirdrop1, transform.position + Vector3.up * 15, Quaternion.Euler(0, 0, 0));
        else if (index == 2)
            _obj = Instantiate(prefabAirdrop2, transform.position + Vector3.up * 15, Quaternion.Euler(0, 0, 0));
        else return;

        _obj.GetComponent<NetworkObject>().Spawn(true);
    }
}
