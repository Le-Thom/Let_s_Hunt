using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealingScript : NetworkBehaviour
{
    [SerializeField] private GameObject animHealing;
    [SerializeField] private Tps_PlayerController playerController;
    [ClientRpc]
    public void PlayAnimHealingClientRpc()
    {
        StartCoroutine(StopHealingAnim());
    }

    private IEnumerator StopHealingAnim()
    {
        animHealing.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animHealing.SetActive(false);
        playerController.ChangeStateToIdle();
    }
}
