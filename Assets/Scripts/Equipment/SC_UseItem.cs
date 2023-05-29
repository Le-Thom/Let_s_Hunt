using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SC_UseItem : MonoBehaviour
{
    public virtual void UseItem(Tps_PlayerController player, sc_Equipment equipment)
    {
        _UseItem(player, equipment);
        Destroy(this);
    }
    protected virtual void _UseItem(Tps_PlayerController player, sc_Equipment equipment) { }
}
