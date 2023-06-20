using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OnDamageBox : NetworkBehaviour
{
    public int damage;
    public GameObject hitParticule;
    public Vector2 offsetHit = new(1, -2);
    public FMODUnity.EventReference hitMonsterRef;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonsterHitCollider>(out MonsterHitCollider component))
        {
            if (IsOwner)
            {
                component.MonsterGetHitServerRpc(damage);
            }
            FMODUnity.RuntimeManager.PlayOneShot(hitMonsterRef, transform.position);
            component.FeedbackMonsterHit();
            Vector3 positionOfHit = other.ClosestPoint(transform.position);

            positionOfHit = new(positionOfHit.x, offsetHit.x, other.transform.position.z + offsetHit.y);

            GameObject hitFeedback = Instantiate(hitParticule, positionOfHit, Quaternion.identity);
            Destroy(hitFeedback, 3);
        }
    }
}
    