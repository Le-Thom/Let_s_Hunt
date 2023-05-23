using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HunterHitCollider : MonoBehaviour
{
    [SerializeField] private ScS_PlayerData playerData;
    private void Awake() => playerData = ScS_PlayerData.Instance;

    [SerializeField] private Equipment equipment1, equipment2;

    /// <summary>
    /// If collider got hit, transfert info to player.
    /// </summary>
    [ClientRpc]
    public void TransfertHitInfoToClient(int indexPlayer, int Damage)
    {
        // if isOwner then change his hp, else only change the healthbar
        if (indexPlayer == playerData.monitor.index) playerData.ChangeHp(Damage);
        
        HealthBarManager.Instance.ChangeHealthBar(indexPlayer, Damage);
    }

    /// <summary>
    /// If hunter get hit. (positive if heal, negative if damage)
    /// </summary>
    /// <param name="value"></param>
    public void GetHit(int value) => playerData.ChangeHp(value);

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
