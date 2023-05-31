using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class MainMenu_Link : MonoBehaviour
{
    //=======
    //VARIABLES
    //=======

    [Header("Twitch Side")]
    [SerializeField] private Button connectToTwitchButton;
    [SerializeField] private TMP_InputField channelName;
    [SerializeField] private GameObject twitchChatUI;

    private string channel => channelName.text.ToLower();

    [SerializeField] private Button createRoomButton;

    [Header("Soldat Side")]
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private TMP_InputField joinCodeInputField;

    [Header("Script / Component")]
    [SerializeField] private GetTwitchData_Script twitch_Script;
    [SerializeField] private Testing_Relay network_Script;
    [SerializeField] private List<GameObject> listOfLoadingIcon = new();



    //=======
    //MONOBEHAVIOUR
    //=======

    private void Awake()
    {
        if(AllTheComponentAreInTheScene())
        {
            //Adding Event To Button

            connectToTwitchButton.onClick.AddListener(ConnectToChosenTwitchChannel);

            joinRoomButton.onClick.AddListener(JoinStreamerRoom);
        }
        else
        {
            Debug.LogError("Script Missing In Scene");
            this.enabled = false;
        }
    }
    private void OnEnable()
    {
        twitch_Script.startingConnectionEvent.AddListener(WhenTheConnectionIsStartListener);

        twitch_Script.onConnectionSuccess.AddListener(WhenTheConnectionIsSuccessfullListener);

        twitch_Script.onForcedDisconect.AddListener(WhenConnectionIsFailed);

        createRoomButton.onClick.AddListener(CreateARoom);

        network_Script.onJoinRoomFailed.AddListener(() => SetLoadingIcon(false, LoadingIcon.JoinGameLoading));
        network_Script.onJoinRoomSuccess.AddListener(() => SetLoadingIcon(false, LoadingIcon.JoinGameLoading));

        network_Script.onCreateRoomFailed.AddListener(() => SetLoadingIcon(false, LoadingIcon.CreateRoom));
        network_Script.onCreateRoomSuccess.AddListener(() => SetLoadingIcon(false, LoadingIcon.CreateRoom));
    }
    private void OnDisable()
    {
        twitch_Script.startingConnectionEvent.RemoveListener(WhenTheConnectionIsStartListener);

        twitch_Script.onConnectionSuccess.RemoveListener(WhenTheConnectionIsSuccessfullListener);

        twitch_Script.onForcedDisconect.RemoveListener(WhenConnectionIsFailed);

        createRoomButton.onClick.RemoveListener(CreateARoom);

        network_Script.onJoinRoomFailed.RemoveListener(() => SetLoadingIcon(false, LoadingIcon.JoinGameLoading));
        network_Script.onJoinRoomSuccess.RemoveListener(() => SetLoadingIcon(false, LoadingIcon.JoinGameLoading));

        network_Script.onCreateRoomFailed.RemoveListener(() => SetLoadingIcon(false, LoadingIcon.CreateRoom));
        network_Script.onCreateRoomSuccess.RemoveListener(() => SetLoadingIcon(false, LoadingIcon.CreateRoom));
    }
    //=======
    //FONCTION
    //=======
    public void CancelTwitchLoading()
    {
        twitch_Script.DisconnectedTwitch();
    }
    private void WhenTheConnectionIsStartListener()
    {
        SetLoadingIcon(true, LoadingIcon.TwitchLoading);
        StartCoroutine(TimerBeforeTimeout());
        connectToTwitchButton.enabled = false;
    }
    private void WhenTheConnectionIsSuccessfullListener()
    {
        SetLoadingIcon(false, LoadingIcon.TwitchLoading);
        StopCoroutine(TimerBeforeTimeout());
        connectToTwitchButton.enabled = true;

        ShowTchatMenu();
    }
    private void WhenConnectionIsFailed()
    {
        SetLoadingIcon(false, LoadingIcon.TwitchLoading);
        connectToTwitchButton.enabled = true;
    }
    private bool AllTheComponentAreInTheScene()
    {
        bool value = true;

        if(twitch_Script == null || network_Script == null)
        {
            value = false;
        }

        return value;
    }
    private void ConnectToChosenTwitchChannel()
    {
        twitch_Script.ConnectToTwitch(channel);
    }
    private void JoinStreamerRoom()
    {
        string joinCode = joinCodeInputField.text;

        SetLoadingIcon(true, LoadingIcon.JoinGameLoading);

        network_Script.JoinRelay(joinCode);
    }
    private void SetLoadingIcon(bool active, LoadingIcon loadingIcon)
    {
        listOfLoadingIcon[((int)loadingIcon)].SetActive(active);
    }
    private IEnumerator TimerBeforeTimeout()
    {
        yield return new WaitForSeconds(25);
        if (!twitch_Script.IsTwitchConnected())
        {
            twitch_Script.DisconnectedTwitch();
            SetLoadingIcon(false, LoadingIcon.TwitchLoading);
        }
    }
    private void ShowTchatMenu()
    {
        twitchChatUI.SetActive(true);
        //Adding Event on lost connection
    }
    private void CreateARoom()
    {
        if (twitch_Script.IsTwitchConnected()) 
        { 
            SetLoadingIcon(true, LoadingIcon.CreateRoom);
            network_Script.ConnectToRelay();
        }
        else Debug.LogWarning("Failed : Twitch Not Connected");
    }
}
enum LoadingIcon
{
    TwitchLoading, JoinGameLoading, CreateRoom
}
