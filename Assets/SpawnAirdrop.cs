using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnAirdrop : NetworkBehaviour
{
    [SerializeField] private GameObject prefabAirdrop;
    [Button]
    public void Spawn_Airdrop()
    {
        GameObject _obj = Instantiate(prefabAirdrop);
        _obj.GetComponent<NetworkObject>().Spawn(true);
    }
}
