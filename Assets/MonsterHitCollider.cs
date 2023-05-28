using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

public class MonsterHitCollider : NetworkBehaviour

{
    [SerializeField] private MonsterHealth monsterHealthScript;
    public static event Action<int> onMonsterHit;

    [ServerRpc(RequireOwnership = false)]
    public void MonsterGetHitServerRpc(int damage)
    {
        onMonsterHit?.Invoke(damage);
    }
    [Button]
    public void TestingHit()
    {
        MonsterGetHitServerRpc(-5);
    }
}
