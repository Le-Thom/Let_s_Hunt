using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceChangerPlayer : MonoBehaviour
{
    [SerializeField] private LoginCredentials credentials;
    [SerializeField] private int index;
    [SerializeField] private Image image;
    [SerializeField] private Sprite hunterSprite1, hunterSprite2, hunterSprite3, hunterSprite4;
    [SerializeField] private Slider slider;
    [SerializeField] private bool main;
    [SerializeField] private List<VoiceChangerPlayer> voiceChangerPlayers;
    [SerializeField] private bool debug;

    public void SetOption(int indexPlayer, bool self)
    {
        if (self)
        {
            slider.onValueChanged.AddListener(credentials.AdjustVolume);
            Destroy(this);
        }

        index = indexPlayer;

        if (index == 1)
            image.sprite = hunterSprite1;
        else if (index == 2)
            image.sprite = hunterSprite2;
        else if (index == 3)
            image.sprite = hunterSprite3;
        else if (index == 4)
            image.sprite = hunterSprite4;
    }

    private void OnEnable()
    {
        if (debug) return;
        if (index == 0 && main)
        {
            for (int i = 0; i < voiceChangerPlayers.Count; i++)
            {
                voiceChangerPlayers[i].SetOption(i + 1, false);
            }
        }
    }

    public void ChangeVolume(float volume)
    {
        int _volume = Mathf.RoundToInt(volume);
        credentials.AdjustVolumeOfOther(index, _volume);
    }
}
