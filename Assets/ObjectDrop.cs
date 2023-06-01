using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(sc_SpriteMesh))]
[RequireComponent(typeof(SphereCollider))]
public class ObjectDrop : InteractableObject
{

    [SerializeField] private GameObject parent;

    private sc_SpriteMesh spriteMesh => GetComponent<sc_SpriteMesh>();
    private SphereCollider sphereCollider => GetComponent<SphereCollider>();

    [SerializeField] private int nb;
    [SerializeField] private sc_Object sc_object;

    [SerializeField] private HunterHitCollider hunterFollowed;

    private float time = 0;
    [SerializeField] private float BreathingSpeed;
    [SerializeField] private float BreathingAmplitude;
    [SerializeField] private AnimationCurve BreathingCurve;

    [SerializeField] private HunterHitCollider playerController;
    private Coroutine attractCoroutine;

    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private Equipment equipment;

    private HunterHitCollider spawnFromPlayerCheck;

    [SerializeField] private LayerMask layerHitOnSpawn;
    [SerializeField] private SC_sc_Object listEquipment;
    private void Start()
    {
        isInteractable = true;
        sphereCollider.radius = 2f;
        onCanPickUp.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        Anim_Breathing();
    }

    [ClientRpc]
    public void SetUpObjClientRpc(int _object, HunterHitCollider isFromPlayer)
    {
        sc_object = listEquipment.objects[_object];
        nb = 1;
        spriteMesh.sprite = sc_object.objectSprite;
        spriteMesh.m_material = sc_object.objectMaterial;

        spawnFromPlayerCheck = isFromPlayer;

        sphereCollider.enabled = true;
    }

    private void Anim_Breathing()
    {
        time += Time.deltaTime;
        // transform.localPosition += Vector3.up * BreathingAmplitude * BreathingCurve.Evaluate(time * BreathingSpeed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (_hunterCollider.IsOwner)
            {
                if (spawnFromPlayerCheck == _hunterCollider) return;
                EquipmentVerification(_hunterCollider);
            } 
            
            else
            {
                StopCoroutine(attractCoroutine);
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
                if (!_hunterCollider.IsOwner) return;
                EquipmentVerification2(_hunterCollider);
            }
            return;
        }
    }

    private void EquipmentVerification(HunterHitCollider _hunterCollider)
    {
        if (!_hunterCollider.IsOwner) return;

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
        if (!_hunterCollider.IsOwner) return;

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
        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);

        GetComponent<NetworkObject>().Despawn(true);
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (_hunterCollider == hunterFollowed) StopCoroutine(attractCoroutine);

            if (!_hunterCollider.IsOwner) return;
            spawnFromPlayerCheck = null;

            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);
        }
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
    [ClientRpc]
    public override void InteractClientRpc()
    {
        if (IsHost && sc_object.GetType() == typeof(sc_Equipment))
        {
            int _howMuchLeft = equipment.AddEquipment(sc_object as sc_Equipment ,nb);
            if (_howMuchLeft > 0) nb = _howMuchLeft;
            else Destroy(parent);
        }
    }

    private void AddingEquipment(Equipment _equipment)
    {
        int _howMuchLeft = _equipment.AddEquipment(nb);
        if (_howMuchLeft > 0)
        {
            nb = _howMuchLeft;
            EquipmentVerification(hunterFollowed);
        }
        else Destroy(parent);
    }

    
}
