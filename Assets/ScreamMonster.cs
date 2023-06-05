using FMODUnity;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScreamMonster : Singleton<ScreamMonster>
{
    public delegate void EventsScream(Vector3 position, float timer);
    public EventsScream delegateEventsScream;
    [SerializeField] private EventReference SoundReference;
    [SerializeField] private float timerScream;
    [SerializeField] private float cooldownScream;
    public bool canScreaming = true;
    [ClientRpc]
    public void Scream(Vector3 position)
    {
        if (!canScreaming) return;
        canScreaming = false;

        transform.position = position;

        // play particle

        delegateEventsScream?.Invoke(position, timerScream);
        RuntimeManager.PlayOneShot(SoundReference, position);
        StartCoroutine(TimerScream(cooldownScream));
    }

    private IEnumerator TimerScream(float timer)
    {
        yield return new WaitForSeconds(timer);
        canScreaming = true;
    }
}
