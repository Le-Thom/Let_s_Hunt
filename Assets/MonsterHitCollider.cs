using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using NaughtyAttributes;
using System;
using System.Threading.Tasks;

public class MonsterHitCollider : NetworkBehaviour

{
    private bool canGetHit = true;
    public static event Action<int> onMonsterHit;

    [ServerRpc(RequireOwnership = false)]
    public void MonsterGetHitServerRpc(int damage)
    {
        if(canGetHit)
        onMonsterHit?.Invoke(damage);
    }
    [Button]
    public void TestingHit()
    {
        MonsterGetHitServerRpc(-5);
    }
    public async void GetMonsterInvincibleForXMiliseconds(int milliseconds)
    {
        if(canGetHit)
        {
            canGetHit = false;
            await Task.Delay(milliseconds);
            canGetHit = true;
        }
    }
}
