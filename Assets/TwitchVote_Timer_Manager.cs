using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TwitchVote_Timer_Manager : Singleton<TwitchVote_Timer_Manager>
{
    [SerializeField] private TextMeshProUGUI timer_text;
    public void UpdateTimerTest(string newText)
    {
        timer_text.text = newText;
    }
}
