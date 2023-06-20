using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class Hunter_DamageZone : NetworkBehaviour
{
    public int damage;
    public float second = 999;
    public Vector2 offsetHit = new(1, -2);
    public Collider trigger;
    public Dictionary<HunterHitCollider, float> lastTimeHit = new();
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterCollider))
        {
            DamageFonction(hunterCollider);
            //lastTimeHit.Add(hunterCollider, 0);
        }
    }
    /*private void Update()
    {
        for(int i = 0; i < lastTimeHit.Count; i++)
        {
            lastTimeHit.Values.ToList()[i] += Time.deltaTime;
            if(lastTimeHit.Values.ToList()[i] > second)
            {
                DamageFonction(lastTimeHit.Keys.ToList()[i]);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterCollider))
        {
            if (lastTimeHit.ContainsKey(hunterCollider))
                lastTimeHit.Remove(hunterCollider);
        }
    }*/
    [ClientRpc]
    public void SetTriggerClientRpc(bool value)
    {
        trigger.enabled = value;
    }
    private void DamageFonction(HunterHitCollider hunterCollider)
    {
        if (IsOwner) 
        { 
            hunterCollider.HunterGetHitClientRpc(damage);
            if (Screenshake_Manager.instance != null) Screenshake_Manager.instance.ScreenshakeSolo(Screenshake.Extra_S);
        }


        hunterCollider.HitFeedback();
        /*
        Collider other = hunterCollider.GetComponentInChildren<Collider>(); 
        Vector3 positionOfHit = other.ClosestPoint(transform.position);

        positionOfHit = new(positionOfHit.x, offsetHit.x, other.transform.position.z + offsetHit.y);
        GameObject hitFeedback = Instantiate(hunterCollider.hitParticule, positionOfHit, Quaternion.identity);
        Destroy(hitFeedback, 3f);*/
    }
}
