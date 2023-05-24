using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public int GetHpValue() { return Mathf.RoundToInt(slider.value); }

    // to who this bar is owned.
    [SerializeField] private int indexPlayer;
    public void SetIndexPlayer(int _indexPlayer) => _indexPlayer = indexPlayer;
    public int GetIndexPlayer() { return indexPlayer; }
    // Player owner this bar
    [SerializeField] private bool isOwner;

    private void OnEnable()
    {
        slider.value = slider.maxValue;
    }

    public void ChangeValueHealthBar(int value)
    {
        slider.value = value;
    }
    
}
