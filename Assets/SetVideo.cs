using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SetVideo : MonoBehaviour
{
    [SerializeField] private VideoClip streamers, soldat, spectateur;
    [SerializeField] private VideoPlayer video;

    public void SetStreamers()
    {
        video.clip = streamers;
    }
    public void SetSoldats()
    {
        video.clip = soldat;
    }
    public void SetSpectateur()
    {
        video.clip = spectateur;
    }
}
