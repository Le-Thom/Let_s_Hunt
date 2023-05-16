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
    public bool isTheSoldierReady { private set; get; } = false;
    public int soliderState = (int)SoliderState.WaitingForSoliderToJoin;

    //========
    //FONCTION
    //========

    public void WhenSoliderJoin()
    {
        soliderState = (int)SoliderState.SoldierJoined;
        soliderNameUI.text = "Joined !";

        soliderWaitingIcon.SetActive(false);
        soliderJoinedIcon.SetActive(true);
        soliderReadyIcon.SetActive(false);
    }
    public void WhenSoliderIsReady()
    {
        soliderState = (int)SoliderState.SoldierReady;
        soliderNameUI.text = "Joined !";

        soliderWaitingIcon.SetActive(false);
        soliderJoinedIcon.SetActive(false);
        soliderReadyIcon.SetActive(true);
    }
}
enum SoliderState
{
    WaitingForSoliderToJoin, 
    SoldierJoined,
    SoldierReady
}
