using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using NaughtyAttributes;

public class HunterHitCollider : NetworkBehaviour
{
    private NetworkVariable<int> indexPlayer = new NetworkVariable<int>(0);

    // NEED NETWORK HERE FOR INDEX PLAYER

    /// <summary>
    /// If collider got hit, transfert info to player.
    /// </summary>
    [ClientRpc]
    public void HunterGetHitClientRpc(int Damage)
    {
        // change the healthbar
        if (IsHost) return; // Monster don't have this.

        HealthBarManager.Instance.ChangeHealthBar(indexPlayer.Value, Damage); 
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerIdServerRpc(int newPlayerId)
    {
        indexPlayer.Value = newPlayerId;
    }
    public int GetPlayerId()
    {
        return indexPlayer.Value;
    }
}
