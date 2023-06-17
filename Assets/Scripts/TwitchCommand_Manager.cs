using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwitchCommand_Manager : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    private Dictionary<string, UnityEvent<string, string>> commandList = new();

    public UnityEvent<string, string> onChatSendMessage;
    public UnityEvent<string, string> onChatSendVote;
    public UnityEvent<string, string> onChatSendDynamo;

    //========
    //MONOBEHAVIOUR
    //========
    private void Awake()
    {
        commandList.Add("!vote", onChatSendVote);
        commandList.Add("!dynamo", onChatSendDynamo);
        onChatSendVote.AddListener(SortVoteChoice);
    }
    //========
    //FONCTION
    //========
    public void SortTwitchMessage(string user, string message)
    {
        bool commandFound = false;
        foreach (string command in commandList.Keys)
        {
            if (message.Contains(command))
            {
                    commandList.TryGetValue(command, out UnityEvent<string, string> eventOfCommand);
                    if (eventOfCommand != null) eventOfCommand.Invoke(user, message);
                    commandFound = true;
            }
        }
        if (onChatSendMessage != null && message.StartsWith("!") && !commandFound) onChatSendMessage.Invoke(user, message);
    }
    public void SortVoteChoice(string user, string message)
    {
        int voteChoice = -1;

        for(int i = 0; i < 5; i++)
        {
            if (message.Contains(i.ToString()))
            {
                voteChoice = i;
            }
        }
        if (voteChoice == -1 || voteChoice == 4) return;
        TwitchVoting_Manager.addingVote(voteChoice);
    }
}
