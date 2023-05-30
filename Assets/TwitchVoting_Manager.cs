using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchVoting_Manager : MonoBehaviour
{
    //========
    //VARIABLES
    //========
    public static Action<string> onStartingVote;
    public static Action<int> addingVote;

    public List<sc_TwitchVote> listOfAllPossibleVote = new();
    public List<sc_TwitchVote> listOfCurrentVote = new();

    //========
    //MONOBEHAVIOUR
    //========

    private void OnEnable()
    {
        addingVote += OnVote;
    }
    private void OnDisable()
    {
        addingVote -= OnVote;
    }

    //========
    //FONCTION
    //========

    public void StartTwitchVote()
    {
        ResetVoteCount();

        List<sc_TwitchVote> listOfCurrentVote = ChoseXRandomFromList(listOfAllPossibleVote, 4);

        foreach(sc_TwitchVote twitchVote in listOfCurrentVote)
        {
            onStartingVote?.Invoke(twitchVote.name);
        }
    }
    public void OnVote(int voteNumber)
    {
        listOfCurrentVote[voteNumber].voteCount++;
    }
    private void ResetVoteCount()
    {
        foreach(sc_TwitchVote twitchVote in listOfAllPossibleVote)
        {
            twitchVote.voteCount = 0;
        }
    }
    private List<sc_TwitchVote> ChoseXRandomFromList(List<sc_TwitchVote> listOfAllPossibility, int numberToTake)
    {
        List<sc_TwitchVote> chosenList = new(numberToTake);

        for(int i = 0; i < numberToTake; i++)
        {
            sc_TwitchVote randomFromList = listOfAllPossibility[UnityEngine.Random.Range(0, listOfAllPossibility.Count)];
            chosenList.Add(randomFromList);
        }

        return chosenList;
    }
}
