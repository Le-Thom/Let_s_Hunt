using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BrunoMikoski.AnimationSequencer;

public class TwitchVote_Timer_Manager : Singleton<TwitchVote_Timer_Manager>
{
    [SerializeField] private TextMeshProUGUI timer_text;
    [SerializeField] private AnimationSequencerController anim;
    public void UpdateTimerTest(string newText)
    {
        timer_text.text = newText;
        anim.ResetComplete();
        anim.Play();

    }
}
