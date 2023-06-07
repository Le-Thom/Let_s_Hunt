using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoteRef_UI : MonoBehaviour
{
    [SerializeField] private Slider voteBar;
    [SerializeField] private TextMeshProUGUI voteNameText;
    [SerializeField] private TextMeshProUGUI pourcentageOfVote;
    [SerializeField] private TextMeshProUGUI voteIdText;

    public void InitVoteUI(string voteName, int voteId)
    {
        voteBar.value = 0;
        voteBar.maxValue = 100;

        pourcentageOfVote.text = "0";
        voteNameText.text = voteName;
        voteIdText.text = voteId.ToString();
    }
    public void UpdatePourcentageOfVote(int newValue)
    {
        voteBar.value = newValue;
        pourcentageOfVote.text = newValue.ToString() + "%";
    }
}
