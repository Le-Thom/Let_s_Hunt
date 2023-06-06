using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "sc_TwitchVote")]
public class sc_TwitchVote : ScriptableObject
{
    public int voteCount = 0;
    public string nameOfTheVote = "blank";
    public TwitchVote twitchVote; 
}
