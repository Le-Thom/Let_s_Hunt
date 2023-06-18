using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ObjectDrop : InteractableObject
{

    [SerializeField] private GameObject parent;

    [SerializeField] private sc_SpriteMesh spriteMesh;
    [SerializeField] private SphereCollider sphereCollider;

    [SerializeField] private NetworkVariable<int> nb;
    [SerializeField] private sc_Object sc_object;

    [SerializeField] private HunterHitCollider hunterFollowed;


    [SerializeField] private HunterHitCollider playerController;
    private Coroutine attractCoroutine;

    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private Equipment equipment;

    private bool isFromHost;

    [SerializeField] private SC_sc_Object listEquipment;
    private void Start()
    {
        isInteractable = true;
        sphereCollider.radius = 2f;
        onCanPickUp.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    [ClientRpc]
    public void SetUpObjClientRpc(int _object, bool isFromPlayer, int wichPlayer)
    {
        sc_object = listEquipment.objects[_object];
        SetNbServerRpc(1);
        spriteMesh.sprite = sc_object.objectSprite;
        spriteMesh.m_material = sc_object.objectMaterial;

        if (wichPlayer == ScS_PlayerData.Instance.monitor.index)
        {
            isFromHost = isFromPlayer;
        }

        sphereCollider.enabled = true;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetNbServerRpc(int _nb)
    {
        nb.Value = _nb;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (_hunterCollider.IsOwner )
            {
                if (isFromHost || IsHost) return;

                if (sc_object.GetType() == typeof(sc_Equipment))
                    EquipmentVerification(_hunterCollider);

                else
                    Tps_PlayerController.Instance.interactableObjects.Add(this);
            } 
            
            else
            {
                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                hunterFollowed = null;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (attractCoroutine == null)
        {
            if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
            {
                if (!_hunterCollider.IsOwner  || IsHost) return;


                if (sc_object.GetType() == typeof(sc_Equipment))
                    EquipmentVerification2(_hunterCollider);
            }
            return;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (_hunterCollider == hunterFollowed) StopCoroutine(attractCoroutine);

            if (!_hunterCollider.IsOwner || IsHost) return;
            isFromHost = false;

            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);
        }
    }

    private void EquipmentVerification(HunterHitCollider _hunterCollider)
    {
        if (!_hunterCollider.IsOwner || IsHost) return;

        if (sc_object.GetType() == typeof(sc_Equipment))
        {
            Equipment _equipment1 = Tps_PlayerController.Instance.GetEquipment(1);
            Equipment _equipment2 = Tps_PlayerController.Instance.GetEquipment(2);

            if (_equipment1.GetEquipment() == sc_object && _equipment1.nbInInventaire < _equipment1.GetEquipment().maxStackEquipment)
            {

                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment1));
            }

            else if (_equipment2.GetEquipment() == sc_object && _equipment2.nbInInventaire < _equipment2.GetEquipment().maxStackEquipment)
            {
                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment2));
            }

            else
            {
                Tps_PlayerController.Instance.interactableObjects.Add(this);
                return;
            }

            return;
        }

        Tps_PlayerController.Instance.interactableObjects.Add(this);
    }
    private void EquipmentVerification2(HunterHitCollider _hunterCollider)
    {
        if (!_hunterCollider.IsOwner || IsHost) return;

        if (sc_object.GetType() == typeof(sc_Equipment))
        {
            Equipment _equipment1 = Tps_PlayerController.Instance.GetEquipment(1);
            Equipment _equipment2 = Tps_PlayerController.Instance.GetEquipment(2);

            if (_equipment1.GetEquipment() == sc_object && _equipment1.nbInInventaire < _equipment1.GetEquipment().maxStackEquipment)
            {

                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment1));
            }

            else if (_equipment2.GetEquipment() == sc_object && _equipment2.nbInInventaire < _equipment2.GetEquipment().maxStackEquipment)
            {
                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment2));
            }

            return;
        }
    }

    private void OnDestroy()
    {
        if (IsHost) return;
        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);
    }

    // we never know what can happen.
    bool debug_dontPass = false;
    private IEnumerator AttractToPlayer(Equipment _equipment)
    {
        if (onCanPickUp.active) onCanPickUp.SetActive(false);

        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);

        while ((parent.transform.position.x >= hunterFollowed.transform.position.x + 0.15f ||
            parent.transform.position.x <= hunterFollowed.transform.position.x - 0.15f) &&
            (parent.transform.position.y >= hunterFollowed.transform.position.y + 0.15f ||
            parent.transform.position.y <= hunterFollowed.transform.position.y - 0.15f)
            )
        {
            // if something go taken while in attract then stop coroutine
            if (_equipment.GetEquipment() != sc_object || _equipment.nbInInventaire >= _equipment.GetEquipment().maxStackEquipment)
            {
                debug_dontPass = true;
                break;
            }

            parent.transform.position = Vector3.Slerp(transform.position, hunterFollowed.transform.position, Time.deltaTime * 2);
            yield return null;

        }


        if (!debug_dontPass)
            AddingEquipment(_equipment);
        else
            EquipmentVerification(hunterFollowed);

        debug_dontPass = false;
    }

    public override void IsClosestToInteract()
    {
        if (!onCanPickUp.active) onCanPickUp.SetActive(true);

        if (sc_object.GetType() == typeof(sc_Equipment))
        {
            equipment = Tps_PlayerController.Instance.IsOneOfEquipmentEmpty();
            if (equipment == null) Tps_PlayerController.Instance.interactableObjects.Remove(this);
        }
    }
    public override void StopBeingTheClosest()
    {
        if (onCanPickUp.active) onCanPickUp.SetActive(false);
    }

    public override void Interact()
    {
        if (sc_object.GetType() == typeof(sc_Equipment))
        {
            equipment.AddEquipment(sc_object as sc_Equipment, nb.Value);
        }
        else
        {
            if (sc_object.objectName == "DamageUp")
            {
                Tps_PlayerController.Instance.damage = Mathf.RoundToInt(Tps_PlayerController.Instance.damage + Tps_PlayerController.Instance.damageMultiplicator);
                Tps_PlayerController.Instance.damageMultiplicator = (Tps_PlayerController.Instance.damageMultiplicator) * 0.9f ;
            }
            else if (sc_object.objectName == "Axe")
            {
                if (Tps_PlayerController.Instance.weaponIsActive == false)
                {
                    Tps_PlayerController.Instance.weaponIsActive = true;
                }
            }
            else if (sc_object.objectName == "Dash")
            {
                Tps_PlayerController.Instance.dash += 10;
            }
        }
        DestroyObjServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjServerRpc()
    {
        Destroy(gameObject);
    }

    private void AddingEquipment(Equipment _equipment)
    {
        int _howMuchLeft = _equipment.AddEquipment(nb.Value);
        if (_howMuchLeft > 0)
        {
            nb.Value = _howMuchLeft;
            EquipmentVerification(hunterFollowed);
        }
        else DestroyObjServerRpc();
    }

    
}
