using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OnDamageBox : NetworkBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonsterHitCollider>(out MonsterHitCollider component))
        {
            if(IsOwner)
            component.MonsterGetHitServerRpc(damage);
            Debug.LogError("yes");
        }
    }
}
