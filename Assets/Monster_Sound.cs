using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Monster_Sound : NetworkBehaviour
{
    [SerializeField] private Monster_Movement monster_Movement;
    [SerializeField] FMODUnity.EventReference cac;
    [SerializeField] FMODUnity.EventReference aoe;
    [SerializeField] FMODUnity.EventReference dash;
    [SerializeField] FMODUnity.EventReference blackout;

    [ServerRpc] 
    public void SpawnSoundServerRpc(int i)
    {
        SpawnSoundClientRpc(monster_Movement.transform.position, i);
    }
    [ClientRpc]
    private void SpawnSoundClientRpc(Vector3 position, int i)
    {
        switch (i)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot(cac, position);
                break;
            case 1:
                FMODUnity.RuntimeManager.PlayOneShot(aoe, position);
                break;
            case 2:
                FMODUnity.RuntimeManager.PlayOneShot(dash, position);
                break;
            case 3:
                FMODUnity.RuntimeManager.PlayOneShot(blackout, position);
                break;
        }
    }
}
