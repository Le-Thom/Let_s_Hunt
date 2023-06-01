using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Airdrop : InteractableObject
{
    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private SC_sc_Object listEquipment;
    [SerializeField] private int nbInBox;
    [SerializeField] private NetworkList<int> whatIsInside = new NetworkList<int>() { };
    [SerializeField] private NetworkList<Vector3> throwObj = new NetworkList<Vector3>() { };
    public void Start()
    {
        isInteractable = true;
        onCanPickUp.SetActive(false);

        if (!IsHost) return;

        for (int i = 0; i < nbInBox; i++)
        {
            int rng = Random.Range(0, listEquipment.objects.Count);
            whatIsInside.Add(rng);

            Vector3 throwRng = new Vector3(Random.Range(-2f, 2f) * 100, 0, Random.Range(-2f, 2f) * 100) * Time.deltaTime + Vector3.up * 4;
            throwObj.Add(throwRng);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (!_hunterCollider.IsOwner) return;
            Tps_PlayerController.Instance.interactableObjects.Add(this);
            //_hunterCollider.GetComponentInParent<Tps_PlayerController>()
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

    private void OnDestroy()
    {
        GetComponent<NetworkObject>().Despawn(true);
        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);
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

    [ServerRpc(RequireOwnership = false)]
    public override void InteractServerRpc()
    {
        for (int i = 0; i < nbInBox; i++)
        {
            GameObject _drop = Resources.Load<GameObject>("DropObject");

            GameObject _obj = Instantiate(_drop, transform.position, Quaternion.Euler(0, 0, 0));
            _obj.GetComponent<NetworkObject>().Spawn(true);

            _obj.GetComponentInChildren<ObjectDrop>().SetUpObjClientRpc(nbInBox, false, -1);

            _obj.GetComponent<Rigidbody>().velocity += throwObj[i];
        }

        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);

        Destroy(gameObject);
    }
    #endregion


}
