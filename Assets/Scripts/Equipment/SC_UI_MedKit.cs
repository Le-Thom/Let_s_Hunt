using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SC_UI_MedKit : SC_UseItem
{
    [ClientRpc]
    protected override void _UseItemClientRpc(Vector3 player, int equipment, Vector2 direction)
    {
        if (!IsHost) return;

        Tps_PlayerController.Instance.ChangeStateToPlayerHealing();
    }
}
