using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fonction Of The Medkit
/// </summary>
public class Medkit_Script
{
    //========
    //VARIABLES
    //========

    /// <summary>
    /// Ref To PlayerController for getting the GetStates Fonction
    /// </summary>
    private Tps_PlayerController playerController;
    /// <summary>
    /// Player To Heal
    /// </summary>
    private int playerId = 0;

    private int amountToHeal = 0;
    private int secondBeforeHeal = 0;
    private bool isMedkitUsed = false;
    private bool canUseThisMedkit = true;

    //========
    //FONCTION
    //========
    public Medkit_Script(Tps_PlayerController Tps_PlayerController, int PlayerId, int AmountToHeal, int SecondBeforeHeal)
    {
        playerController = Tps_PlayerController;
        playerId = PlayerId;
        amountToHeal = AmountToHeal;
        secondBeforeHeal = SecondBeforeHeal;

        isMedkitUsed = false;
        canUseThisMedkit = true;
    }
    public async void OnUse()
    {
        //minisecond = seconde / 1000
        await Task.Delay(secondBeforeHeal * 1000);
        //Check If The Player Is In Healing State

        if (playerController.GetCurrentState() == AkarisuMD.Player.StateId.HEALING && canUseThisMedkit)
        {
            //If Yes then 
            HealthBarManager.Instance.ChangeHealthBar(playerId, amountToHeal);
            isMedkitUsed = true;
            //Remove Equipement

        }
        else
        {
            Debug.Log("Heal Failed");
        }
    }
    public bool DidPlayerFinishUsingThisMedkit()
    {
        return isMedkitUsed;
    }
    public void SetPlayerCanUseMedkit(bool newValue)
    {
        canUseThisMedkit = newValue;
    }
}
