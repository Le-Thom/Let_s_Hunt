using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Radio : InteractableObject
{
    // Reference
    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private GameObject uiRadio;


    // Monobehaviour
    private void Start()
    {
        isInteractable = true;
        onCanPickUp.SetActive(false);
        uiRadio.SetActive(false);
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

            if (uiRadio.active) uiRadio.SetActive(false);
        }
    }

    // Public fonction
    public override void IsClosestToInteract()
    {
        if (!onCanPickUp.active) onCanPickUp.SetActive(true);
    }
    public override void StopBeingTheClosest()
    {
        if (onCanPickUp.active) onCanPickUp.SetActive(false);
    }
    public override void Interact()
    {
        uiRadio.SetActive(!uiRadio.active);
    }

    // Private fonction
}
