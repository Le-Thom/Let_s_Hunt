using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Airdrop : InteractableObject
{
    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private sc_Object[] possibleDrops;
    private void Start()
    {
        isInteractable = true;
        onCanPickUp.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            Tps_PlayerController.Instance.interactableObjects.Add(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);
        }
    }

    #region interaction
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
        for (int i = 0; i < 2; i++)
        {
            int rng = Random.Range(0, possibleDrops.Length);
            sc_Object sc_Object = possibleDrops[rng];

            GameObject _drop = Instantiate(Resources.Load<GameObject>("DropObject"), transform.position, Quaternion.Euler(0, 0, 0));
            _drop.GetComponentInChildren<ObjectDrop>().SetUpObj(sc_Object, null);

            _drop.GetComponent<Rigidbody>().velocity += new Vector3(Random.Range(-2f, 2f) * 100, 0, Random.Range(-2f, 2f) * 100) * Time.deltaTime + Vector3.up * 4;
        }

        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);

        Destroy(gameObject);
    }
    #endregion


}
