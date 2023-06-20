using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
        voteIdText.text = (voteId + 1).ToString() + ":";
    }
    public void UpdatePourcentageOfVote(int newValue)
    {
        voteBar.value = newValue;
        pourcentageOfVote.text = newValue.ToString();
        switch(newValue.ToString().Length)
        {
            case 3:
                pourcentageOfVote.fontSize = 30;
                break;
            case 4:
                pourcentageOfVote.fontSize = 24;
                break;
        }
    }
    public void AutoDestroy()
    {
        transform.DOScale(0, 1f).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject));
    }
}
