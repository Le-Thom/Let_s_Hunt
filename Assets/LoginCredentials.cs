using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using VivoxUnity;

public class LoginCredentials : MonoBehaviour
{
    VivoxUnity.Client client;
    private string userName;
    private Uri server = new Uri("https://unity.vivox.com/appconfig/90718-let_s-74999-udash");
    private string domain = "mtu1xp.vivox.com";
    private string issuer = "90718-let_s-74999-udash";
    private string tokenKey = "WtDDeT2Gy7vgGPthJiencgS3rsD0HvyV";
    private TimeSpan timeSpan = TimeSpan.FromSeconds(90);
    private AsyncCallback loginCallback;

    private ILoginSession loginSession;
    private IChannelSession channelSession;


    private string channelName;

    private void Awake()
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
        ChannelId channelId = new ChannelId(issuer, channelName, domain, ChannelType.NonPositional);
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
                break;
            case ConnectionState.Disconnecting:
                Debug.Log($"{source.Channel.Name} Disconnecting");
                break;
            case ConnectionState.Disconnected:
                Debug.Log($"{source.Channel.Name} Disconnected");
                break;
        }
    }
    public void Leave_Channel()
    {
        channelSession.Disconnect();
        loginSession.DeleteChannelSession(new ChannelId(issuer, channelName, domain));
    }

    #endregion
}
