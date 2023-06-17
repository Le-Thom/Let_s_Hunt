using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image image;
    [SerializeField] private Sprite hunterSprite1, hunterSprite2, hunterSprite3, hunterSprite4;
    [SerializeField] private float shakeDuration = 0.8f;
    [SerializeField] VoiceChangerPlayer voiceVolume;
    public int GetHpValue() { return Mathf.RoundToInt(slider.value); }

    // to who this bar is owned.
    [SerializeField] private int indexPlayer;
    public void SetIndexPlayer(int _newIndexPlayer) 
    {
        indexPlayer = _newIndexPlayer; 

        if (indexPlayer == 1)
            image.sprite = hunterSprite1;
        else if (indexPlayer == 2)
            image.sprite = hunterSprite2;
        else if (indexPlayer == 3)
            image.sprite = hunterSprite3;
        else if (indexPlayer == 4)
            image.sprite = hunterSprite4;

        if (indexPlayer == ScS_PlayerData.Instance.monitor.index)
            voiceVolume.SetOption(indexPlayer, true);
        else voiceVolume.SetOption(indexPlayer, false);
    }
    public int GetIndexPlayer() { return indexPlayer; }

    private void OnEnable()
    {
        slider.value = slider.maxValue;
    }

    public void ChangeValueHealthBar(int value)
    {
        slider.value = value;
        StartCoroutine(Shaking());
    }

    private IEnumerator Shaking()
    {
        Vector3 _startPosition = transform.position;
        float _elapsedTime = 0f;

        while (_elapsedTime < shakeDuration)
        {
            _elapsedTime += Time.deltaTime;
            transform.position = _startPosition + Random.insideUnitSphere;
            yield return null;
        }

        transform.position = _startPosition;
    }
    
}
