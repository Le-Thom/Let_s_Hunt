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


    private float time = 0;
    [SerializeField] private float BreathingSpeed;
    [SerializeField] private float BreathingAmplitude;
    [SerializeField] private AnimationCurve BreathingCurve;

    [SerializeField] private List<ObjectDrop> equipmentDropList = new();
    [SerializeField] private HunterHitCollider playerController;
    private Coroutine attractCoroutine;

    [SerializeField] private GameObject onCanPickUp;
    [SerializeField] private Equipment equipment;


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

    public void SetUpObj(sc_Object _object)
    {
        sc_object = _object;
        nb = 1;
        spriteMesh.sprite = _object.objectSprite;
        spriteMesh.m_material = _object.objectMaterial;

        sphereCollider.enabled = true;
    }

    private void Anim_Breathing()
    {
        time += Time.deltaTime;
        transform.localPosition += Vector3.up * BreathingAmplitude * BreathingCurve.Evaluate(time * BreathingSpeed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ObjectDrop>(out ObjectDrop equipmentDrop))
        {
            equipmentDropList.Add(equipmentDrop);
        }

        else if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _playerController))
        {
            Tps_PlayerController.Instance.interactableObjects.Add(this);

            if (sc_object.GetType() == typeof(sc_Equipment))
            {
                Equipment _equipment = Tps_PlayerController.Instance.IsOneOfEquipmentEmpty();
                if (_equipment == null) return;

                
                if (Tps_PlayerController.Instance.GetEquipment(1).GetEquipment() == sc_object) { StartCoroutineAttract(_playerController); }
                else if (Tps_PlayerController.Instance.GetEquipment(1).GetEquipment() == sc_object) { StartCoroutineAttract(_playerController); }
            }
        }
    }
    private void StartCoroutineAttract(HunterHitCollider _playerController)
    {
        if (attractCoroutine != null) StopCoroutine(attractCoroutine);
        attractCoroutine = null;

        playerController = _playerController;
        attractCoroutine = StartCoroutine(AttractToPlayer());
    }

    private void OnDestroy()
    {
        if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
            Tps_PlayerController.Instance.interactableObjects.Remove(this);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _playerController))
        {
            if (Tps_PlayerController.Instance.interactableObjects.Contains(this))
                Tps_PlayerController.Instance.interactableObjects.Remove(this);
        }
    }

    private IEnumerator AttractToPlayer()
    {
        while ((transform.position.x >= playerController.transform.position.x + 0.05f ||
            transform.position.x <= playerController.transform.position.x - 0.05f) &&
            (transform.position.y >= playerController.transform.position.y + 0.05f ||
            transform.position.y <= playerController.transform.position.y - 0.05f)
            )
        {
            transform.position = Vector3.Slerp(transform.position, playerController.transform.position, Time.deltaTime);
            yield return null;
        }

        // check owner
        AddingEquipment();
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

    private void AddingEquipment()
    {
        int _howMuchLeft = equipment.AddEquipment(nb);
        if (_howMuchLeft > 0) nb = _howMuchLeft;
        else Destroy(parent);
    }
}
