using FMODUnity;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScreamMonster : NetworkBehaviour
{
    [SerializeField] private EventReference SoundReference;
    [SerializeField] private float timerScream;
    [SerializeField] private float cooldownScream;
    public bool canScreaming = true;
    [ClientRpc]
    public void ScreamClientRpc(Vector3 position)
    {
        if (!canScreaming) return;
        canScreaming = false;

        transform.position = position;

        // play particle

        RuntimeManager.PlayOneShot(SoundReference, position);
        StartCoroutine(TimerScream(cooldownScream));

        if (IsHost) return;

        Tps_PlayerController.Instance.MonsterScream(position, cooldownScream);
    }

    private IEnumerator TimerScream(float timer)
    {
        yield return new WaitForSeconds(timer);
        canScreaming = true;
    }
}
