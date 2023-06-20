using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Monster_Sound : NetworkBehaviour
{
    [SerializeField] private Monster_Movement monster_Movement; 
    [ServerRpc] 
    public void SpawnSoundServerRpc(string path)
    {
        SpawnSoundClientRpc(monster_Movement.transform.position, path);
    }
    [ClientRpc]
    private void SpawnSoundClientRpc(Vector3 position, string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, position);
    }
}
