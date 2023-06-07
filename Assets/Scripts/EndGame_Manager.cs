using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EndGame_Manager : NetworkBehaviour
{
    //=========
    //VARIABLE
    //=========

    [SerializeField] private GameObject winningCanvas;
    [SerializeField] private GameObject losingCanvas;

    //========
    //MONOBEHAVIOUR
    //========

    private void OnEnable()
    {
        MonsterHealth.whenTheMonsterDied += OnSoldierWinningClientRpc;
        TimeManager.onEndTimer += OnMonsterWinningClientRpc;
    }

    private void OnDisable()
    {
        MonsterHealth.whenTheMonsterDied -= OnSoldierWinningClientRpc;
        TimeManager.onEndTimer -= OnMonsterWinningClientRpc;
    }

    //=========
    //FONCTION
    //=========

    /// <summary>
    /// Call When The Soldier Win in the Animator
    /// </summary>
    [ClientRpc]
    public void OnSoldierWinningClientRpc()
    {
        if(IsHost)
        {
            losingCanvas.SetActive(true);
        }
        else
        {
            winningCanvas.SetActive(true);
        }
        Time.timeScale = 0;
    }
    /// <summary>
    /// Call When The Monster Win in the Animator
    /// </summary>
    [ClientRpc]
    public void OnMonsterWinningClientRpc()
    {
        if (IsHost)
        {
            winningCanvas.SetActive(true);
        }
        else
        {
            losingCanvas.SetActive(true);
        }
        Time.timeScale = 0;
    }
}
