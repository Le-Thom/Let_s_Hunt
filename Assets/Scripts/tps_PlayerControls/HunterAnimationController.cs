using BrunoMikoski.AnimationSequencer;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HunterAnimationController : NetworkBehaviour
{
    [SerializeField] private Tps_PlayerController playerController;
    private bool isOwner 
    { 
        get 
        {
            if (playerController != null) return playerController.enabled;
            else return false;
        } 
    }

    [SerializeField] private GameObject anim_Healing;
    [SerializeField] private FMODUnity.EventReference audio_Healing;
    [SerializeField] private FMODUnity.EventReference audio_Atk1;
    [SerializeField] private FMODUnity.EventReference audio_Atk2;
    [SerializeField] private FMODUnity.EventReference audio_Death;
    [SerializeField] private FMODUnity.EventReference audio_Dodge;
    [SerializeField] private FMODUnity.EventReference audio_Get_Hit;
    [SerializeField] private FMODUnity.EventReference audio_Revive;

    //==============================================================================================================
    #region PRIVATE FONCTION
    //==============================================================================================================


    private void ChangeStateToIdle() 
    {
        if(isOwner)
        playerController.ChangeStateToIdle(); 
    }
    private void StartDodge() 
    {
        if (isOwner)
            playerController.StartDodge(); 
    }
    private void EndDodge() 
    {
        if (isOwner)
            playerController.EndDodge(); 
    }
    private void ATK1() { if (isOwner) playerController.Atk1(); }
    private void ATK2() { if (isOwner) playerController.Atk2(); }
    private void StartHealing() { 
        anim_Healing.SetActive(true);
        AnimationSequencerController _asc = anim_Healing.GetComponentInChildren<AnimationSequencerController>();
        _asc.ResetToInitialState();
        _asc.Play();
    }
    private void EndHealing() {
        anim_Healing.GetComponentInChildren<AnimationSequencerController>().OnFinishedEvent.AddListener(EndAnimHealing);
        Debug.Log("Pass Heal");
        if (!isOwner) return; 

        playerController.playerData.ChangeHp(10);
        HealthBarManager.Instance.ChangeHealthBar(playerController.playerData.monitor.index, 10);
    }
    private void EndAnimHealing()
    {
        anim_Healing.GetComponentInChildren<AnimationSequencerController>().OnFinishedEvent.RemoveAllListeners();
        anim_Healing.SetActive(false);
    }


    // AUDIO
    [ClientRpc]
    public void PlayAudioHealingClientRpc(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Healing, position);
    }
    [ClientRpc]
    public void PlayAudioAtk1ClientRpc(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Atk1, position);
    }
    [ClientRpc]
    public void PlayAudioAtk2ClientRpc(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Atk2, position);
    }
    [ClientRpc]
    public void PlayAudioDeathClientRpc(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Death, position);
    }
    [ClientRpc]
    public void PlayAudioReviveClientRpc(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Revive, position);
    }
    [ClientRpc]
    public void PlayAudioGetHitClientRpc(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Get_Hit, position);
    }
    [ClientRpc]
    public void PlayAudioDodgeClientRpc(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audio_Dodge, position);
    }

    #endregion
    //==============================================================================================================
}
