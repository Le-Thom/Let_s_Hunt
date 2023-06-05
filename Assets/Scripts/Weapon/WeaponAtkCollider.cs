using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAtkCollider : MonoBehaviour
{
    [SerializeField] private Weapon target;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonsterHitCollider>(out MonsterHitCollider component))
        {
            target.listTarget.Add(component);
        }
    }
}
