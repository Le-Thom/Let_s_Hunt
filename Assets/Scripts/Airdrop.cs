using GameplayIngredients.Rigs;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Airdrop : InteractableObject
{
    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private SC_sc_Object listEquipment, listGlobal;
    [SerializeField] private FMODUnity.EventReference PickUp;
    public void Start()
    {
        isInteractable = true;
        onCanPickUp.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (!_hunterCollider.IsOwner || IsHost) return;
            Tps_PlayerController.Instance.interactableObjects.Add(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (!_hunterCollider.IsOwner || IsHost) return;
            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);
        }
    }

    private void OnDestroy()
    {
        if (IsHost) return;
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
        for (int i = 0; i < listEquipment.objects.Count; i++)
        {
            GameObject _drop = Resources.Load<GameObject>("DropObject");

            GameObject _obj = Instantiate(_drop, transform.position, Quaternion.Euler(0, 0, 0));
            _obj.GetComponent<NetworkObject>().Spawn(true);

            for (int y = 0; y < listGlobal.objects.Count; y++)
            {
                if (listGlobal.objects[y] == listEquipment.objects[i])
                {
                    _obj.GetComponentInChildren<ObjectDrop>().SetUpObjClientRpc(y, false, -1);
                    break;
                }
            }

            Vector3 _throwRng = new Vector3(Random.Range(-2f, 2f) * 100, 0, Random.Range(-2f, 2f) * 100) * Time.deltaTime + Vector3.up * 4;
            _obj.GetComponent<Rigidbody>().velocity += _throwRng;
        }

        Destroy(gameObject);
        GetComponent<NetworkObject>().Despawn(true);
        AudioPickUpClientRpc();
    }

    [ClientRpc]
    private void AudioPickUpClientRpc()
    {
        FMODUnity.RuntimeManager.PlayOneShot(PickUp, transform.position);
    }

    #endregion


}
