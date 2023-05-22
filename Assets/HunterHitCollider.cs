using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterHitCollider : MonoBehaviour
{
    [SerializeField] private ScS_PlayerData playerData;
    private void Awake() => playerData = ScS_PlayerData.Instance;

    [SerializeField] private Equipment equipment1, equipment2;

    /// <summary>
    /// If collider got hit, transfert info to player.
    /// </summary>
    public void TransfertHitInfoToClient()
    {
        // Multiplayer info here
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
