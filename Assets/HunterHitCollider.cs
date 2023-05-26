using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using NaughtyAttributes;

public class HunterHitCollider : NetworkBehaviour
{
    [SerializeField] private Equipment equipment1, equipment2;

    private NetworkVariable<int> indexPlayer = new NetworkVariable<int>(0);
    public List<EquipmentDrop> equipmentDropsLists = new();

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

    public Equipment GetEquipment(sc_Equipment equipment) 
    {
        if (equipment1.GetEquipment() == equipment)
            return equipment1;
        else if (equipment2.GetEquipment() == equipment)
            return equipment2;
        else return null;
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
