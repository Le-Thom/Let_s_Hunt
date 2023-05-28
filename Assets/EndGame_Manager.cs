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
    }
}
