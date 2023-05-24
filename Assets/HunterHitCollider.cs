using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HunterHitCollider : NetworkBehaviour
{
    [SerializeField] private Equipment equipment1, equipment2;

    private int indexPlayer;


    // NEED NETWORK HERE FOR INDEX PLAYER


    /// <summary>
    /// If collider got hit, transfert info to player.
    /// </summary>
    // [ClientRpc]
    public void HunterGetHit(int Damage)
    {
        // change the healthbar
        if (IsHost) return; // Monster don't have this.

        HealthBarManager.Instance.ChangeHealthBar(indexPlayer, Damage); 
    }

    public Equipment GetEquipment(sc_Equipment equipment) 
    {
        if (equipment1.GetEquipment() == equipment)
            return equipment1;
        else if (equipment2.GetEquipment() == equipment)
            return equipment2;
        else return null;
    }

    public List<EquipmentDrop> equipmentDropsLists = new();
}
