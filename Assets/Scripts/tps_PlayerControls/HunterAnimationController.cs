using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAnimationController : MonoBehaviour
{
    [SerializeField] private Tps_PlayerController playerController;
    [SerializeField] private bool isOwner;

    //==============================================================================================================
    #region PRIVATE FONCTION
    //==============================================================================================================

    private void ChangeStateToIdle() => playerController.ChangeStateToIdle();
    private void StartDodge() => playerController.StartDodge();
    private void EndDodge() => playerController.EndDodge();
    private void ATK1() => playerController.Atk1();
    private void ATK2() => playerController.Atk2();
    private void Revive() => playerController.Revive();

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
