using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sc_SpriteMesh))]
[RequireComponent(typeof(SphereCollider))]
public class EquipmentDrop : InteractableObject
{
    public EquipmentDrop(sc_Equipment _equipment)
    {
        equipment = _equipment;
    }

    [SerializeField] private int nb;
    [SerializeField] private sc_Equipment equipment;


    private float time = 0;
    [SerializeField] private float BreathingSpeed;
    [SerializeField] private float BreathingAmplitude;
    [SerializeField] private AnimationCurve BreathingCurve;

    [SerializeField] private List<EquipmentDrop> equipmentDropList = new();
    [SerializeField] private HunterHitCollider playerController;
    private Coroutine attractCoroutine;
    private Equipment _equipment;

    private void Update()
    {
        Anim_Breathing();
    }
    private void Anim_Breathing()
    {
        time += Time.deltaTime;
        transform.localPosition += Vector3.up * BreathingAmplitude * BreathingCurve.Evaluate(time * BreathingSpeed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EquipmentDrop>(out EquipmentDrop equipmentDrop))
        {
            equipmentDropList.Add(equipmentDrop);
        }

        else if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider _playerController))
        {
            Tps_PlayerController.Instance.interactableObjects.Add(this);
            Equipment _equipment = Tps_PlayerController.Instance.IsOneOfEquipmentEmpty();
            if (_equipment == null) return;
            if (_equipment.GetEquipment() != equipment) return;

            if (attractCoroutine != null) StopCoroutine(attractCoroutine);
            attractCoroutine = null;

            playerController = _playerController;
            attractCoroutine = StartCoroutine(AttractToPlayer());
        }
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

        // check if is owner then continue
        int _howMuchLeft = _equipment.AddEquipment(nb);
        if (_howMuchLeft > 0) nb = _howMuchLeft;
        else Destroy(gameObject);
    }

    public override void IsClosestToInteract()
    {
        base.IsClosestToInteract();
    }
    public override void StopBeingTheClosest()
    {
        base.StopBeingTheClosest();
    }
    public override void Interact()
    {
        
    }
}
