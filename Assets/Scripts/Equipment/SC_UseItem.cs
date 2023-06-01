using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class SC_UseItem : NetworkBehaviour
{
    public virtual void UseItem(Vector3 player, int equipment, Vector2 direction)
    {
        _UseItemServerRpc(player, equipment, direction);
    }
    [ServerRpc]
    protected virtual void _UseItemServerRpc(Vector3 player, int equipment, Vector2 direction) { }
}
