using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoldierLobby : MonoBehaviour
{
    //========
    //VARIABLES
    //========
    public TextMeshProUGUI soliderNameUI;
    public GameObject soliderWaitingIcon;
    public GameObject soliderJoinedIcon;
    public GameObject soliderReadyIcon;
    public int soliderState = (int)SoliderState.WaitingForSoliderToJoin;

    //========
    //FONCTION
    //========
    public void WhenWaitingForSoldier()
    {
        soliderState = (int)SoliderState.WaitingForSoliderToJoin;
        soliderNameUI.text = "En attente d'un joueur ...";

        soliderWaitingIcon.SetActive(true);
        soliderJoinedIcon.SetActive(false);
        soliderReadyIcon.SetActive(false);
    }

    public void WhenSoliderJoin()
    {
        soliderState = (int)SoliderState.SoldierJoined;
        soliderNameUI.text = "Connecté !";

        soliderWaitingIcon.SetActive(false);
        soliderJoinedIcon.SetActive(true);
        soliderReadyIcon.SetActive(false);
    }
    public void WhenSoliderIsReady()
    {
        soliderState = (int)SoliderState.SoldierReady;
        soliderNameUI.text = "Prêt !";

        soliderWaitingIcon.SetActive(false);
        soliderJoinedIcon.SetActive(true);
        soliderReadyIcon.SetActive(true);
    }
}
enum SoliderState
{
    WaitingForSoliderToJoin, 
    SoldierJoined,
    SoldierReady
}
