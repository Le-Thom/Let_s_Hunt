using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SetVideo : MonoBehaviour
{
    [SerializeField] private VideoClip streamers, soldat, spectateur;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private Slider timeSlider;

    private void OnEnable()
    {
        video.clip = spectateur;
        OnChangeVideo();
    }
    public bool onMouseOver;
    public void OnSliderChange(float value)
    {
        if (onMouseOver)
            video.time = value;
    }
    public void SetStreamers()
    {
        video.clip = streamers;
        OnChangeVideo();
    }
    public void SetSoldats()
    {
        video.clip = soldat;
        OnChangeVideo();
    }
    public void SetSpectateur()
    {
        video.clip = spectateur;
        OnChangeVideo();
    }
    private void Update()
    {
        if (!onMouseOver)
        {
            timeSlider.value = (float)video.time;
        }
        else if (!video.isPlaying && video.time <= video.clip.length && !onMouseOver)
            video.Play();
    }
    private void OnChangeVideo()
    {
        timeSlider.maxValue = (float)video.clip.length;
        video.time = 0;
        timeSlider.value = 0;
        video.Play();
    }
}
