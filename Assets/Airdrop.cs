using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Airdrop : InteractableObject
{
    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private sc_Object[] possibleDrops;
    [SerializeField] private int nbInBox;
    [SerializeField] private NetworkList<int> whatIsInside = new NetworkList<int>() { };
    [SerializeField] private NetworkList<Vector3> throwObj = new NetworkList<Vector3>() { };
    public override void OnNetworkSpawn()
    {
        isInteractable = true;
        onCanPickUp.SetActive(false);

        if (!IsHost) return;

        for (int i = 0; i < nbInBox; i++)
        {
            int rng = Random.Range(0, possibleDrops.Length);
            whatIsInside.Add(rng);

            Vector3 throwRng = new Vector3(Random.Range(-2f, 2f) * 100, 0, Random.Range(-2f, 2f) * 100) * Time.deltaTime + Vector3.up * 4;
            throwObj.Add(throwRng);
        }
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

    private void OnDestroy()
    {
        GetComponent<NetworkObject>().Despawn(true);
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

    [ClientRpc]
    public override void InteractClientRpc()
    {
        for (int i = 0; i < nbInBox; i++)
        {
            sc_Object sc_Object = possibleDrops[whatIsInside[i]];

            GameObject _drop = Instantiate(Resources.Load<GameObject>("DropObject"), transform.position, Quaternion.Euler(0, 0, 0));
            _drop.GetComponentInChildren<ObjectDrop>().SetUpObj(sc_Object, null);

            _drop.GetComponent<Rigidbody>().velocity += throwObj[i];
        }

        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);

        Destroy(gameObject);
    }
    #endregion


}
