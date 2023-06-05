using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class SC_UseItem : NetworkBehaviour
{
    public virtual void UseItem(Vector3 player, int equipment, Vector2 direction)
    {
        _UseItemClientRpc(player, equipment, direction);
    }
    [ClientRpc]
    protected virtual void _UseItemClientRpc(Vector3 player, int equipment, Vector2 direction) { }
}
