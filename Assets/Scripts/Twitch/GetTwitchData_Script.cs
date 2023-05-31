using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;
using System.IO;
using System;
using System.Threading.Tasks;

public class GetTwitchData_Script : Singleton<GetTwitchData_Script>
{
    //========
    //VARIABLES
    //========
    [Header("Event")]
    //When a member of the chat write a message
    public UnityEvent<string, string> OnChatMessage;
    public UnityEvent startingConnectionEvent;
    public UnityEvent onConnectionSuccess;
    public UnityEvent onForcedDisconect;

    [Header("Twitch Connect")]
    private TcpClient Twitch;

    //Get The Info From Twitch
    private StreamReader Reader;
    //Send Info For Twitch
    private StreamWriter Writer;

    [Header("Twitch Info")]
    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;

    [Header("Phantom User")]
    //The user that gonna connect to the stream
    string User = "ElMisterYesYes";
    string OAuth = "oauth:s8dodjpznd9t4z30a1duqi4l35ohfe";

    //The Twitch Channel were we gonna get the chat
    private string channel = "Laink";

    private bool isConnectionStarted = false;
    private bool isConnectedToTwitch = false;
    //========
    //MONOBEHAVIOUR
    //========
    private void Awake()
    {
        DisconnectedTwitch();
    }
    private void OnDisable()
    {
        DisconnectedTwitch();
    }
    private void OnApplicationQuit()
    {
        try 
        { 
            DisconnectedTwitch(); 
        }
        catch { }
    }

    private void Update()
    {
        if (!isConnectionStarted) return;
        //If The Streamer Get Disconnected From Twitch, We try to reconnect him
        if(!Twitch.Connected)
        {
            ConnectToTwitch(channel);
            print("Deconnected From Twitch");
        }
        if(Twitch.Available > 0)
        {
            //Twitch send a message
            string message = Reader.ReadLine();

            //If the message is a chat message
            if(message.Contains("PRIVMSG"))
            {
                //We separate the username / chatter to the message 
                int splitPoint = message.IndexOf("!");
                string chatter = message.Substring(1, splitPoint - 1);

                splitPoint = message.IndexOf(":", 1);
                string msg = message.Substring(splitPoint + 1);

                OnChatMessage?.Invoke(chatter, msg);
            }
            if(message.Contains("JOIN"))
            {
                onConnectionSuccess?.Invoke();
                isConnectedToTwitch = true;
            }
            print(message);
        }
    }
    //========
    //FONCTION
    //========
    public void ConnectToTwitch(string Channel)
    {
        Twitch = new TcpClient(URL, PORT);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());

        channel = Channel;

        //We ask from misterYesYes to connect on the Twitch
        Writer.WriteLine("PASS " + OAuth);
        Writer.WriteLine("NICK " + User.ToLower());

        //We ask from misterYesYes to join the Channel of the streamer
        Writer.WriteLine("JOIN #" + Channel.ToLower());
        Writer.Flush();

        startingConnectionEvent?.Invoke();

        Debug.Log("Starting The Connection To Twitch");

        if(!isConnectionStarted)
        {
            TimeOutConnection();
            isConnectedToTwitch = false;
        }

        isConnectionStarted = true;
    }
    /// <summary>
    /// Assure the deconnection of Twitch
    /// </summary>
    public void DisconnectedTwitch()
    {
        isConnectionStarted = false;
        if (Twitch != null && Twitch.Connected)
        {
            if (Writer != null)
            {
                Writer.WriteLine("PART #" + channel.ToLower());
                Writer.Flush();
            }

            if (Reader != null)
                Reader.Close();

            if (Writer != null)
                Writer.Close();

            Twitch.Close();

            onForcedDisconect?.Invoke();
        }
    }
    public bool IsTwitchConnected()
    {
        if (Twitch == null) 
            return false;
        else
        return Twitch.Connected;
    }
    public string GetChannelName()
    {
        return channel;
    }
    public async void TimeOutConnection()
    {
        //If the connect didn't start proprely, we try each 5 sec
        for(int i = 0; i<= 6; i++)
        {
            await Task.Delay(5000);
            if(isConnectionStarted && !isConnectedToTwitch) ConnectToTwitch(channel);
        }
        if(isConnectionStarted && !isConnectedToTwitch)
        {
            //Time out
            DisconnectedTwitch();
        }
    }
}
