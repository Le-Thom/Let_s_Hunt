using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;
using FMODUnity;

[RequireComponent(typeof(SphereCollider))]
public class Radio : InteractableObject
{

    // Reference
    [SerializeField] private GameObject onCanPickUp, notAvailable;

    // private
    [SerializeField] private bool available;
    [SerializeField] private float unavailableTimer = 30f;

    [SerializeField] private ParticleSystem particle1, particle2;

    [SerializeField] private FMODUnity.EventReference UsingAudio, ResetAudio;

    // Monobehaviour
    private void OnEnable()
    {
        TwitchVoting_Manager.onStartingVote += AvailableTimer;
    }
    private void OnDisable()
    {
        TwitchVoting_Manager.onStartingVote -= AvailableTimer;
    }
    private void Start()
    {
        isInteractable = true;
        available = true;
        onCanPickUp.SetActive(false);
        notAvailable.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (!_hunterCollider.IsOwner) return;

            Tps_PlayerController.Instance.interactableObjects.Add(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (!_hunterCollider.IsOwner) return;

            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);

        }
    }

    // Public fonction
    public override void IsClosestToInteract()
    {
        if (available && !onCanPickUp.active) onCanPickUp.SetActive(true);
        else if (!available && !notAvailable.active) notAvailable.SetActive(true);
    }
    public override void StopBeingTheClosest()
    {
        if (onCanPickUp.active) onCanPickUp.SetActive(false);
        if (notAvailable.active) notAvailable.SetActive(false);
    }
    [ServerRpc(RequireOwnership = false)]
    public override void InteractServerRpc()
    {
        base.InteractServerRpc();
        StartVote();
        print("radioZ");
        InteractClientRpc();
    }
    [ClientRpc]
    public override void InteractClientRpc()
    {
        if (!available) return;

        particle1.Play();
        particle2.Play();

        available = false;

        RuntimeManager.PlayOneShot(UsingAudio, transform.position);
        print("radio");
    }

    // Private fonction

    private void StartVote()
    {
        TwitchVoting_Manager.Instance.StartTwitchVote();
    }

    private async void AvailableTimer()
    {
        if (onCanPickUp.active) onCanPickUp.SetActive(false);
        if (!available && !notAvailable.active) notAvailable.SetActive(true);
        TwitchVote_Timer_Manager.Instance.UpdateTimerTest(unavailableTimer.ToString());
        for(int i = 1; i < unavailableTimer; i++)
        {
            await System.Threading.Tasks.Task.Delay(1000);
            TwitchVote_Timer_Manager.Instance.UpdateTimerTest((unavailableTimer - i).ToString());
        }
        SetAvailableClientRpc();
        TwitchVoting_Manager.Instance.EndTwitchVote();
        TwitchVote_Timer_Manager.Instance.UpdateTimerTest("");
    }

    [ClientRpc]
    public void SetAvailableClientRpc()
    {
        available = true;
        RuntimeManager.PlayOneShot(ResetAudio, transform.position);
    }
}
