using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sc_SpriteMesh))]
[RequireComponent(typeof(SphereCollider))]
public class ObjectDrop : InteractableObject
{
    public ObjectDrop(sc_Object _object)
    {
    }

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
    private bool spawnFromPlayerCheck;


    private void Start()
    {
        sphereCollider.radius = 2f;
        onCanPickUp.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        Anim_Breathing();
    }

    public void SetUpObj(sc_Object _object, bool isFromPlayer)
    {
        sc_object = _object;
        nb = 1;
        spriteMesh.sprite = _object.objectSprite;
        spriteMesh.m_material = _object.objectMaterial;

        sphereCollider.enabled = true;

        spawnFromPlayerCheck = isFromPlayer;
    }

    private void Anim_Breathing()
    {
        time += Time.deltaTime;
        transform.localPosition += Vector3.up * BreathingAmplitude * BreathingCurve.Evaluate(time * BreathingSpeed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _hunterCollider))
        {
            if (spawnFromPlayerCheck) return;
            EquipmentVerification(_hunterCollider);
        }
    }

    private void EquipmentVerification(HunterHitCollider _hunterCollider)
    {
        // If !isOwner then return

        if (sc_object.GetType() == typeof(sc_Equipment))
        {
            Debug.Log("Pass 1");

            Equipment _equipment1 = Tps_PlayerController.Instance.GetEquipment(1);
            Equipment _equipment2 = Tps_PlayerController.Instance.GetEquipment(2);

            if (_equipment1.GetEquipment() == sc_object && _equipment1.nbInInventaire < _equipment1.GetEquipment().maxStackEquipment)
            {
                Debug.Log("Pass 2");

                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                ChangeAttract(hunterFollowed);
                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment1));
            }

            else if (_equipment2.GetEquipment() == sc_object && _equipment2.nbInInventaire < _equipment2.GetEquipment().maxStackEquipment)
            {
                Debug.Log("Pass 2 bis");
                if (attractCoroutine != null) StopCoroutine(attractCoroutine);
                attractCoroutine = null;

                ChangeAttract(hunterFollowed);
                hunterFollowed = _hunterCollider;

                attractCoroutine = StartCoroutine(AttractToPlayer(_equipment2));
            }

            else
            {

                Debug.Log("Pass 4");

                Tps_PlayerController.Instance.interactableObjects.Add(this);
                return;
            }

            return;
        }

        Debug.Log("Pass 5");
        Tps_PlayerController.Instance.interactableObjects.Add(this);
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
            spawnFromPlayerCheck = false;

            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);

            if (_hunterCollider == hunterFollowed) StopCoroutine(attractCoroutine);
        }
    }

    // we never know what can happen.
    bool debug_dontPass = false;
    private IEnumerator AttractToPlayer(Equipment _equipment)
    {
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


        Debug.Log("Pass 3");

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
            Debug.Log("Pass 7");
            int _howMuchLeft = equipment.AddEquipment(sc_object as sc_Equipment ,nb);
            if (_howMuchLeft > 0) nb = _howMuchLeft;
            else Destroy(parent);
            Debug.Log($"Pass 9 {_howMuchLeft}");
        }
    }

    private void AddingEquipment(Equipment _equipment)
    {
        Debug.Log("Pass 5");
        int _howMuchLeft = _equipment.AddEquipment(nb);
        if (_howMuchLeft > 0)
        {
            nb = _howMuchLeft;
            EquipmentVerification(hunterFollowed);
        }
        else Destroy(parent);
    }
}
