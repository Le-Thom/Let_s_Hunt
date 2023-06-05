using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_UI_MedKit : SC_UseItem
{
    protected override void _UseItem(Tps_PlayerController player, sc_Equipment equipment)
    {
        player.ChangeStateToPlayerHealing();
    }
}
