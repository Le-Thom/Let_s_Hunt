using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class TwitchVoting_Manager : Singleton<TwitchVoting_Manager>
{
    //========
    //VARIABLES
    //========
    public static Action onStartingVote;
    public static Action<int> addingVote;

    public List<sc_TwitchVote> listOfAllPossibleVote = new();
    public List<VoteRef_UI> listOfVoteUI = new();
    public Dictionary<sc_TwitchVote, VoteRef_UI> listOfCurrentVote = new();
    [SerializeField] private GameObject voteUI;
    [SerializeField] private VerticalLayoutGroup voteGroupUI;
    [SerializeField] private int voteTime = 100000;
    private bool isVoteStarted = false;

    [SerializeField] private GameObject countDownVote;

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

    [Button]
    public void StartTwitchVote()
    {
        print("Strating Vote");
        if (isVoteStarted) return;
        ResetVoteCount();
        countDownVote.SetActive(true);

        listOfCurrentVote.Clear();

        isVoteStarted = true;

        List<sc_TwitchVote> listOfChosenVote = listOfAllPossibleVote;


        int voteId = 0;
        foreach (sc_TwitchVote twitchVote in listOfChosenVote)
        {
            Transform newVoteUI = Instantiate(voteUI.transform, voteGroupUI.transform);
            if (newVoteUI.TryGetComponent<VoteRef_UI>(out VoteRef_UI voteRef_UI))
            {
                listOfCurrentVote.Add(twitchVote, voteRef_UI);
                voteRef_UI.InitVoteUI(twitchVote.nameOfTheVote, voteId);
                voteId++;
            }
        }
        onStartingVote?.Invoke();
    }
    public void OnVote(int voteNumber)
    {
        sc_TwitchVote key = listOfCurrentVote.Keys.ToList()[voteNumber];
        key.voteCount++;
        listOfCurrentVote.Values.ToList()[voteNumber].UpdatePourcentageOfVote(key.voteCount);
    }
    public void EndTwitchVote()
    {
        sc_TwitchVote winningVote = null;
        foreach (sc_TwitchVote sc_TwitchVote in listOfCurrentVote.Keys)
        {
            if (winningVote == null)
            {
                winningVote = sc_TwitchVote;
                continue;
            }
            if (sc_TwitchVote.voteCount > winningVote.voteCount)
            {
                winningVote = sc_TwitchVote;
                continue;
            }
        }
        isVoteStarted = false;
        //
        switch (winningVote.twitchVote)
        {
            case TwitchVote.DropMedkit:

                AirdropManager _airdropManager = GameObject.FindFirstObjectByType<AirdropManager>();
                _airdropManager.CallAirdrop(1);

                UI_Message_Manager.Instance.ShowMessage(Color.blue, "Renforcement Materiel en chemin");
                break;
            case TwitchVote.DropWeapon:

                AirdropManager _airdropManager1 = GameObject.FindFirstObjectByType<AirdropManager>();
                _airdropManager1.CallAirdrop(2);

                UI_Message_Manager.Instance.ShowMessage(Color.blue, "Renforcement Materiel en chemin");
                break;
            case TwitchVote.Scream:

                ScreamMonster _screamMonster = GameObject.FindFirstObjectByType<ScreamMonster>();
                Monster_Movement _monster_Movement = GameObject.FindFirstObjectByType<Monster_Movement>();
                _screamMonster.ScreamClientRpc(_monster_Movement.transform.position);

                UI_Message_Manager.Instance.ShowMessage(Color.blue, "Show The Location Of The Monster To Soldier");
                break;
            case TwitchVote.Light:

                SpotlightManager _spotlightManager = GameObject.FindFirstObjectByType<SpotlightManager>();
                _spotlightManager.CallSpotlightOnClientRpc();

                UI_Message_Manager.Instance.ShowMessage(Color.blue, "Activation des spotlights");
                break;
            case TwitchVote.Revive:

                Revive[] revives = FindObjectsByType<Revive>(FindObjectsSortMode.InstanceID);

                foreach(Revive revive in revives)
                {
                    revive._ReviveServerRpc();
                }

                UI_Message_Manager.Instance.ShowMessage(Color.blue, "Reanimation des Soldats en cours");
                break;
        }
        foreach(VoteRef_UI voteRef_UI in listOfCurrentVote.Values.ToList())
        {
            voteRef_UI.AutoDestroy();
        }
        countDownVote.SetActive(false);
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
    [Button]
    private void ReviveAll()
    {
        Revive[] revives = FindObjectsByType<Revive>(FindObjectsSortMode.InstanceID);

        foreach (Revive revive in revives)
        {
            revive._ReviveServerRpc();
        }
    }
}
public enum TwitchVote
{
    DropMedkit, DropWeapon, Scream, Light, Revive
}
