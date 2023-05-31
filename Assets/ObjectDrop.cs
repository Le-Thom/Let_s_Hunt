using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
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

    public void SetUpObj(sc_Object _object, HunterHitCollider isFromPlayer)
    {
        sc_object = _object;
        nb = 1;
        spriteMesh.sprite = _object.objectSprite;
        spriteMesh.m_material = _object.objectMaterial;

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
            if (spawnFromPlayerCheck == _hunterCollider) return;
            EquipmentVerification(_hunterCollider);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (attractCoroutine == null)
        {
            if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
            {
                EquipmentVerification2(_hunterCollider);
            }
            return;
        }
    }

    private void EquipmentVerification(HunterHitCollider _hunterCollider)
    {
        // If !isOwner then return

        if (sc_object.GetType() == typeof(sc_Equipment))
        {
            Equipment _equipment1 = Tps_PlayerController.Instance.GetEquipment(1);
            Equipment _equipment2 = Tps_PlayerController.Instance.GetEquipment(2);

            if (_equipment1.GetEquipment() == sc_object && _equipment1.nbInInventaire < _equipment1.GetEquipment().maxStackEquipment)
            {

                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                ChangeAttract(hunterFollowed);
                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment1));
            }

            else if (_equipment2.GetEquipment() == sc_object && _equipment2.nbInInventaire < _equipment2.GetEquipment().maxStackEquipment)
            {
                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                ChangeAttract(hunterFollowed);
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
        // If !isOwner then return

        if (sc_object.GetType() == typeof(sc_Equipment))
        {
            Equipment _equipment1 = Tps_PlayerController.Instance.GetEquipment(1);
            Equipment _equipment2 = Tps_PlayerController.Instance.GetEquipment(2);

            if (_equipment1.GetEquipment() == sc_object && _equipment1.nbInInventaire < _equipment1.GetEquipment().maxStackEquipment)
            {

                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                ChangeAttract(hunterFollowed);
                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment1));
            }

            else if (_equipment2.GetEquipment() == sc_object && _equipment2.nbInInventaire < _equipment2.GetEquipment().maxStackEquipment)
            {
                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                ChangeAttract(hunterFollowed);
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
    }
    private void OnTriggerExit(Collider other)
    {
        // if !isowner then return;

        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            spawnFromPlayerCheck = null;

            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);

            if (_hunterCollider == hunterFollowed) StopCoroutine(attractCoroutine);
        }
    }

    // we never know what can happen.
    bool debug_dontPass = false;
    private IEnumerator AttractToPlayer(Equipment _equipment)
    {
        if (onCanPickUp.active) onCanPickUp.SetActive(false);

        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);

        CallOnAttract(hunterFollowed);

        while ((transform.position.x >= hunterFollowed.transform.position.x + 0.15f ||
            transform.position.x <= hunterFollowed.transform.position.x - 0.15f) &&
            (transform.position.y >= hunterFollowed.transform.position.y + 0.15f ||
            transform.position.y <= hunterFollowed.transform.position.y - 0.15f)
            )
        {
            // if something go taken while in attract then stop coroutine
            if (_equipment.GetEquipment() != sc_object || _equipment.nbInInventaire >= _equipment.GetEquipment().maxStackEquipment)
            {
                CallOffAttract(hunterFollowed);
                debug_dontPass = true;
                break;
            }

            transform.position = Vector3.Slerp(transform.position, hunterFollowed.transform.position, Time.deltaTime * 2);
            yield return null;

        }


        if (!debug_dontPass)
            AddingEquipment(_equipment);
        else
            EquipmentVerification(hunterFollowed);

        debug_dontPass = false;
    }
    private IEnumerator AttractToPlayer()
    {
        while ((transform.position.x >= hunterFollowed.transform.position.x + 0.05f ||
            transform.position.x <= hunterFollowed.transform.position.x - 0.05f) &&
            (transform.position.y >= hunterFollowed.transform.position.y + 0.05f ||
            transform.position.y <= hunterFollowed.transform.position.y - 0.05f)
            )
        {
            transform.position = Vector3.Slerp(transform.position, hunterFollowed.transform.position, Time.deltaTime);
            yield return null;
        }
    }

    // Need ClientRpc here
    private void CallOffAttract(HunterHitCollider wichPlayer) 
    {
        if (wichPlayer == hunterFollowed) StopCoroutine(attractCoroutine);
    }

    // Need ClientRpc here
    private void ChangeAttract(HunterHitCollider wichPlayer)
    {
        if (wichPlayer != hunterFollowed) StopCoroutine(attractCoroutine);
    }

    // Need ClientRpc here
    private void CallOnAttract(HunterHitCollider wichPlayer)
    {
        // If isowner then return
        return;
        hunterFollowed = wichPlayer;
        attractCoroutine = StartCoroutine(AttractToPlayer());
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
