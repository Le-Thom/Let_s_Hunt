using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAnimationController : MonoBehaviour
{
    [SerializeField] private Tps_PlayerController playerController;
    private bool isOwner => playerController.enabled;

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
    private void Revive() { if (isOwner) playerController.Revive(); }

    // AUDIO

    private void AudioOnFootstep()
    {

    }

    private void AudioOnDodge()
    {

    }

    private void AudioOnATK1()
    {

    }

    private void AudioOnATK2()
    {

    }

    private void AudioOnGetHit()
    {

    }

    private void AudioOnDeath()
    {

    }

    private void AudioOnRevive()
    {

    }

    #endregion
    //==============================================================================================================
}
