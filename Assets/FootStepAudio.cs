using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FootStepAudio : NetworkBehaviour
{
    [SerializeField] private FMODUnity.EventReference audio_Foot_step;
    [SerializeField] private ScS_PlayerData playerData;


    public void PlayAudioFootStepClientRpc()
    {
        if (playerData.monitor.isMoving)
            _PlayAudioFootStepClientRpc();
    }

    [ClientRpc]
    public void _PlayAudioFootStepClientRpc()
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Foot_step, transform.position);
    }
}
