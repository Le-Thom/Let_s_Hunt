using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class TwitchSendToClient_Manager : NetworkBehaviour
{
    //========
    //VARIABLES
    //========

    [SerializeField] private SyncChatWithText syncChatWithText_script;

    //========
    //FONCTIONS
    //========

    [ClientRpc]
    public void SendToHunterChatInfoClientRpc(string user, string message)
    {
        syncChatWithText_script.AddingChatMessageToUI(user, message);
    }
}
