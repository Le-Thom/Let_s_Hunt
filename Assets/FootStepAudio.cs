using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FootStepAudio : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference audio_Foot_step;
    [SerializeField] private ScS_PlayerData playerData;

    [ClientRpc]
    public void PlayAudioFootStepClientRpc()
    {
        if (playerData.monitor.isMoving)
            FMODUnity.RuntimeManager.PlayOneShot(audio_Foot_step, transform.position);
    }
}
