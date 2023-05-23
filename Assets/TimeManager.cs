using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    [SerializeField] private Transform transformDirectionalLight;

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
        int _hours, _minutes, _seconds;

        _hours = Mathf.FloorToInt(value / 3600);
        _minutes = Mathf.FloorToInt(value / 60);
        _seconds = Mathf.FloorToInt(value);

        while (_hours > 23) { _hours -= 23; }
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
        gameSecondElapsed += Time.deltaTime * rationSecondForAnHours;
        realTimeElapsedInSecond += Time.deltaTime;
        SecondRemaining -= Time.deltaTime;
        realTimeElapsed = GetCurrentTimeInGameToDisplay(realTimeElapsedInSecond);
    }

    
}
