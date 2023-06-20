using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using VivoxUnity;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering;

public class LoginCredentials : Singleton<LoginCredentials>
{
    public VivoxUnity.Client client;
    private Uri server = new Uri("https://unity.vivox.com/appconfig/90718-let_s-74999-udash");
    private string domain = "mtu1xp.vivox.com";
    private string issuer = "90718-let_s-74999-udash";
    private string tokenKey = "WtDDeT2Gy7vgGPthJiencgS3rsD0HvyV";
    private TimeSpan timeSpan = TimeSpan.FromSeconds(90);
    private AsyncCallback loginCallback;

    private ILoginSession loginSession;
    private IChannelSession channelSession;


    public bool activeVivox;
    public bool isPositional;
    public List<PositionalChannel> positionalChannel;

    private string userName;
    private string channelName;

    public bool channelConnected;

    private void Start()
    {
        client = new Client();
        client.Uninitialize();
        client.Initialize();
        DontDestroyOnLoad(this);
    }

    private void OnApplicationQuit()
    {
        client.Uninitialize();
    }

    #region Login Methods

    public void Bind_Login_Callback_Listeners(bool bind, ILoginSession loginSesh)
    {
        if (bind)
        { loginSesh.PropertyChanged += Login_Status; }
        else
        { loginSesh.PropertyChanged -= Login_Status; }
    }
    public void SetUserName(string username)
    {
        this.userName = username;
    }
    public void Login()
    {
        if (!activeVivox) return;
        AccountId accountId = new AccountId(issuer, userName, domain);
        loginSession = client.GetLoginSession(accountId);

        Bind_Login_Callback_Listeners(true, loginSession);
        loginSession.BeginLogin(server, loginSession.GetLoginToken(tokenKey, timeSpan), ar =>
        {
            try
            {
                loginSession.EndLogin(ar);
            }
            catch (Exception e)
            {
                Bind_Login_Callback_Listeners(false, loginSession);
                Debug.Log(e.Message);
            }
        });
    }
    private void Login_Status(object sender, PropertyChangedEventArgs loginArgs)
    {
        var source = (ILoginSession)sender;

        switch (source.State)
        {
            case LoginState.LoggingIn:
                Debug.Log("Logging In");
                break;

            case LoginState.LoggedIn:
                Debug.Log($"Logged In {loginSession.LoginSessionId.Name}");

                JoinGame_Manager joinGame = FindAnyObjectByType<JoinGame_Manager>();
                SetChannelName(joinGame.joinCode.Value.ToString());
                JoinChannel();
                break;
            case LoginState.LoggedOut:
                UI_Message_Manager.Instance.ShowMessage(Color.red, "Connection To Voice chat Failed, Please Restart");
                break;

        }
    }
    public void Logout()
    {
        Bind_Login_Callback_Listeners(false, loginSession);
        loginSession.Logout();
    }

    #endregion

    #region Join Channel Methods

    public void Bind_Channel_Callback_Listeners(bool bind, IChannelSession channelSesh)
    {
        if (bind)
        { channelSesh.PropertyChanged += Channel_Status; }
        else
        { channelSesh.PropertyChanged -= Channel_Status; }
    }
    public void SetChannelName(string channelName)
    {
        this.channelName = channelName;
    }
    public void JoinChannel()
    {
        if (!activeVivox) return;

        ChannelId channelId = new ChannelId(issuer, channelName, domain, ChannelType.Positional,
            new Channel3DProperties(50, 10, 0.5f, AudioFadeModel.LinearByDistance));
        channelSession = loginSession.GetChannelSession(channelId);

        Bind_Channel_Callback_Listeners(true, channelSession);
        channelSession.BeginConnect(true, false, true, channelSession.GetConnectToken(tokenKey, timeSpan), ar =>
        {
            try
            {
                channelSession.EndConnect(ar);
            }
            catch (Exception e)
            {
                Bind_Channel_Callback_Listeners(false, channelSession);
                Debug.Log(e.Message);
            }
        });
    }
    private void Channel_Status(object sender, PropertyChangedEventArgs loginArgs)
    {
        var source = (IChannelSession)sender;
        switch (source.ChannelState)
        {
            case ConnectionState.Connecting:
                Debug.Log("Channel Connecting");
                break;
            case ConnectionState.Connected:
                Debug.Log($"{source.Channel.Name} Connected");
                channelConnected = true;

                if (!isPositional) return;

                foreach (var item in positionalChannel)
                {
                    item._Start();
                    item.gameObject.SetActive(false);
                }
                PlayerConnectionManager[] connectionsManager = FindObjectsOfType<PlayerConnectionManager>();
                PlayerConnectionManager currentPlayerConnectionManager = null;
                foreach (PlayerConnectionManager connectionManager in connectionsManager)
                {
                    if (connectionManager.IsOwner)
                    {
                        Debug.Log(connectionManager);
                        currentPlayerConnectionManager = connectionManager;
                        break;
                    }
                }
                if (currentPlayerConnectionManager.playerId.Value != 0)
                {
                    currentPlayerConnectionManager.SetVivoxOn();
                }
                else
                {
                    MonsterVoice.Instance.positionalChannel.gameObject.SetActive(true);
                    MonsterVoice.Instance.positionalChannel.ForceMute();
                }
                try
                {
                    AdjustVolume(5);
                    client.AudioOutputDevices.VolumeAdjustment = 1;
                }
                catch (Exception)
                {

                    throw;
                }
                foreach (var item in channelSession.Participants)
                {
                    try
                    {
                        item.LocalVolumeAdjustment = 5;

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                break;
            case ConnectionState.Disconnecting:
                Debug.Log($"{source.Channel.Name} Disconnecting");
                channelConnected = false;
                break;
            case ConnectionState.Disconnected:
                Debug.Log($"{source.Channel.Name} Disconnected");
                channelConnected = false;
                break;
            default:
                Debug.LogError("Strange");
                break;
        }
    }
    public void Leave_Channel()
    {
        channelSession.Disconnect();
        loginSession.DeleteChannelSession(new ChannelId(issuer, channelName, domain));
    }

    #endregion

    #region VOIP

    public void Update3DPosition(Transform listener, Transform speaker)
    {
        channelSession.Set3DPosition(speaker.position, listener.position, listener.forward, listener.up);
    }

    #endregion

    #region Devices

    public VivoxUnity.IReadOnlyDictionary<string, IAudioDevice> GetAllDevices()
    {
        return client.AudioInputDevices.AvailableDevices;
    }
    public IAudioDevices GetCurrentDevices()
    {
        return client.AudioInputDevices;
    }

    public void ChangeInputDevice(IAudioDevice targetInput = null)
    {
        IAudioDevices intputDevices = client.AudioInputDevices;
        if (targetInput != null && targetInput != client.AudioInputDevices.ActiveDevice)
        {
            client.AudioInputDevices.BeginSetActiveDevice(targetInput, ar =>
            {
                if (ar.IsCompleted)
                {
                    client.AudioInputDevices.EndSetActiveDevice(ar);
                }
            });
        };
    }

    public void AdjustVolume(float value)
    {
        IAudioDevices devices = client.AudioInputDevices;
        // Refresh list of devices to have it up to date
        var ar = devices.BeginRefresh(new AsyncCallback((IAsyncResult result) =>
        {
            // Set the volume for the device
            devices.VolumeAdjustment = Mathf.RoundToInt(value);
        }));
    }
    public void AdjustVolumeOfOther(int indexPlayer, int volume)
    {
        foreach (var participant in channelSession.Participants)
        {
            if (indexPlayer.ToString() == participant.Account.Name)
            {
                participant.LocalVolumeAdjustment = volume;
                break;
            }
        }
    }

    #endregion
}
