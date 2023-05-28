using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Radio : InteractableObject
{
    // Reference
    [SerializeField] private GameObject onCanPickUp, notAvailable;

    // private
    [SerializeField] private bool available;
    [SerializeField] private float unavailableTimer = 120f;

    // Monobehaviour
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
            // if is not owner then return;

            Tps_PlayerController.Instance.interactableObjects.Add(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            // if is not owner then return;

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
    public override void Interact()
    {
        if (!available) return;

        available = false;

        StartVote();
    }

    // Private fonction

    private void StartVote()
    {
        StartCoroutine(AvailableTimer());
    }

    private IEnumerator AvailableTimer()
    {
        if (onCanPickUp.active) onCanPickUp.SetActive(false);
        if (!available && !notAvailable.active) notAvailable.SetActive(true);
        yield return new WaitForSeconds(unavailableTimer);
        available = true;
    }
}
