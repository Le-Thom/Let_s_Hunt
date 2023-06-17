using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MusiqueManager : NetworkBehaviour
{
    public FMODUnity.EventReference Main_menu, inGame, fight;

    private void Start()
    {
        if (IsHost)
        {
            SetMenuMusiqueClientRpc();
        }
    }

    [ClientRpc]
    public void SetMenuMusiqueClientRpc()
    {
        RuntimeManager.PlayOneShot(Main_menu);
    }
    public void SetInGameMusiqueOnlyClient()
    {
        RuntimeManager.PlayOneShot(inGame);
    }
    public void SetFightMusiqueOnlyClient()
    {
        RuntimeManager.PlayOneShot(fight);
    }
}
