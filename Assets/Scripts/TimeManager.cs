using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using DG.Tweening;

public class TimeManager : NetworkBehaviour
{
    public static Action onEndTimer; 
    [SerializeField] private Transform transformDirectionalLight;
    [SerializeField] private TextMeshProUGUI _timerText;


    [SerializeField, ReadOnly] private string timeElapsed;
    [SerializeField, ReadOnly] private string realTimeElapsed;
    [SerializeField, ReadOnly] private string currentTimeInGame;

    private float realTimeElapsedInSecond;
    [SerializeField] private float SecondRemaining;
    [Tooltip("Real hours. from 0 to 23")]
    [SerializeField] private int startingHours, endingHours;
    [Tooltip("In real minute, how much time does the game need to do.")]
    [SerializeField] private float RealTimeForAGameInMinute;
    [SerializeField] private float rationSecondForAnHours;
    [Tooltip("In hour. how much hours elapsed during the game.")]
    [SerializeField] private float inGameTimeInSecond;
    [Tooltip("In second. how much seconds elapsed during the game.")]
    [SerializeField] private float _gameSecondElapsed;
    private bool isTimerFinish = false;
    private float unQuartTemps => RealTimeForAGameInMinute * 60 / 10;
    private float gameSecondElapsed
    {
        get { return _gameSecondElapsed; }
        set {
            _gameSecondElapsed = value;
            inGameTimeInSecond = startingHours * 3600 + value;
            if (inGameTimeInSecond > 86399) inGameTimeInSecond -= 86400;
            timeElapsed = GetTimeElapsedToDisplay(value);
            currentTimeInGame = GetCurrentTimeInGameToDisplay(inGameTimeInSecond);

            if (SecondRemaining < unQuartTemps) transformDirectionalLight.Rotate(-Vector3.right * 90/unQuartTemps * Time.deltaTime * 1.5f, Space.Self);
        }
    }

    /// <summary>
    /// Show time elapsed as a string with format "hours:minutes:seconds".
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string GetTimeElapsedToDisplay(float value)
    {
        int _hours, _minutes, _seconds;

        _hours = Mathf.FloorToInt( value / 3600);
        _minutes = Mathf.FloorToInt(value / 60);
        _seconds = Mathf.FloorToInt(value);

        while (_minutes > 59) { _minutes -= 60; }
        while (_seconds > 59) { _seconds -= 60; }

        string _hoursString;
        if (_hours < 10) { _hoursString = $"0{_hours}"; }
        else _hoursString = $"{_hours}";

        string _minutesString;
        if (_minutes < 10) { _minutesString = $"0{_minutes}"; }
        else _minutesString = $"{_minutes}";

        string _secondsString;
        if (_seconds < 10) { _secondsString = $"0{_seconds}"; }
        else _secondsString = $"{_seconds}";


        string _time = $"{_hoursString}:{_minutesString}:{_secondsString}";
        return _time;
    }

    /// <summary>
    /// Show game time as a string with format "hours:minutes:seconds".
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string GetCurrentTimeInGameToDisplay(float value)
    {
        if (value <= 0)
            return "00:00:00";

        int _hours = (int)(value / 3600) % 24;
        int _minutes = (int)(value / 60) % 60;
        int _seconds = (int)value % 60;

        return _hours.ToString("00") + ":" + _minutes.ToString("00") + ":" + _seconds.ToString("00");

        //_hours = Mathf.FloorToInt(value / 3600);
        //_minutes = Mathf.FloorToInt(value / 60);
        //_seconds = Mathf.FloorToInt(value);

        /*
        while (_hours > 23) { _hours -= 24; }
        while (_minutes > 59) { _minutes -= 60; }
        while (_seconds > 59) { _seconds -= 60; }
        */

        /*
        string _hoursString;
        if (_hours < 10) { _hoursString = $"0{_hours}"; }
        else _hoursString = $"{_hours}";

        string _minutesString;
        if (_minutes < 10) { _minutesString = $"0{_minutes}"; }
        else _minutesString = $"{_minutes}";

        string _secondsString;
        if (_seconds < 10) { _secondsString = $"0{_seconds}"; }
        else _secondsString = $"{_seconds}";*/

        //return string.Format("{0:D2}:{0:D2}:{0:D2}", _hours, _minutes, _seconds);

        //string _time = $"{_hoursString}:{_minutesString}:{_secondsString}";
        //return _time;
    }

    [ClientRpc]
    public void StartTimeClientRpc()
    {
        this.enabled = true;
    }
    [ClientRpc]
    public void RemoveSecondOfTimeClientRpc(float secondsToRemove)
    {
        gameSecondElapsed += secondsToRemove * rationSecondForAnHours;
        realTimeElapsedInSecond += secondsToRemove;
        SecondRemaining -= secondsToRemove;
        realTimeElapsed = GetCurrentTimeInGameToDisplay(realTimeElapsedInSecond);

        FeedbackText();
    } 
    private async void FeedbackText()
    {
        if (_timerText != null)
        {
            _timerText.text = GetCurrentTimeInGameToDisplay(SecondRemaining);

            Color baseTextColor = _timerText.color;

            for (int i = 0; i <= 2; i++)
            {
                await _timerText.DOColor(Color.red, 0.5f).AsyncWaitForCompletion();
                await _timerText.DOColor(baseTextColor, 0.5f).AsyncWaitForCompletion();
            }
        }
    }
    private void Start()
    {
        int _endTimeHour;
        if (startingHours > endingHours) _endTimeHour = endingHours + 24;
        else _endTimeHour = endingHours;

        int _timeInHourInGame = _endTimeHour - startingHours;
        float _totalSecondInGame = _timeInHourInGame * 3600;
        float _totalSecondRealTimeForAGame = RealTimeForAGameInMinute * 60;
        SecondRemaining = _totalSecondRealTimeForAGame;

        rationSecondForAnHours = _totalSecondInGame / _totalSecondRealTimeForAGame;
    }
    private void Update()
    {
        //if (!IsOwner) return;
        gameSecondElapsed += Time.deltaTime * rationSecondForAnHours;
        realTimeElapsedInSecond += Time.deltaTime;
        SecondRemaining -= Time.deltaTime;
        realTimeElapsed = GetCurrentTimeInGameToDisplay(realTimeElapsedInSecond);

        if (_timerText != null) 
        {
            _timerText.text = GetCurrentTimeInGameToDisplay(SecondRemaining);
        }

        if(SecondRemaining <= 0 && !isTimerFinish)
        {
            onEndTimer?.Invoke();
            isTimerFinish = true;
            print("End Of The Game, Monster Win");
        }
    }
}
