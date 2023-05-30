using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwitchCommand_Manager : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    private Dictionary<string, UnityEvent<string>> commandList = new();
    public UnityEvent<string> onChatSendPosition;
    public UnityEvent<string> onChatSendStrat;
    public UnityEvent<string> onChatSendVote;

    //========
    //MONOBEHAVIOUR
    //========
    private void Awake()
    {
        commandList.Add("!pos", onChatSendPosition);
        commandList.Add("!strat", onChatSendStrat);
        commandList.Add("!vote", onChatSendVote);
    }
    //========
    //FONCTION
    //========
    public void SortTwitchMessage(string user, string message)
    {
        foreach(string command in commandList.Keys)
        {
            if(message.Contains(command))
            {
                commandList.TryGetValue(command, out UnityEvent<string> eventOfCommand);
                if (eventOfCommand != null) eventOfCommand.Invoke(message);
            }
        }
    }
}
