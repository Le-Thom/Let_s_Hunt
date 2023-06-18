using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FootStepAudio : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference audio_Foot_step;

    [ClientRpc]
    public void PlayAudioFootStepClientRpc()
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Foot_step, transform.position);
    }
}
