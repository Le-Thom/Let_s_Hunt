using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter_DamageZone : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterCollider))
        {
            hunterCollider.HunterGetHitClientRpc(damage);
        }
    }
}
