using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private ScS_PlayerData playerData;

    // to who this bar is owned.
    [SerializeField] private int indexPlayer;
    public void SetIndexPlayer(int _indexPlayer) => _indexPlayer = indexPlayer;
    // Player owner this bar
    [SerializeField] private bool isOwner;

    private void Start()
    {
        playerData = ScS_PlayerData.Instance;
    }

    private void Update()
    {
        if (playerData == null) Debug.LogError("PlayerData not found");

        // if player is owner of this bar then modify his hp bar value to his hp bar
        if (isOwner) {
            if (playerData.inGameDataValue.hp != slider.value) ChangeValueHealthBar(playerData.inGameDataValue.hp);
        }

        // else... need multi info

    }

    private void ChangeValueHealthBar(int value)
    {
        slider.value = value;
    }
    
}
