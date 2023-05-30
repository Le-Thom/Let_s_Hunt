using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_UI_Flare : SC_UseItem
{
    protected override void _UseItem(Tps_PlayerController player, sc_Equipment equipment)
    {
        GameObject _objSpawn = player._Instantiate(equipment.prefab_Object, player.transform.position + Vector3.up * 1.5f, Quaternion.Euler(0, 0, 0));
        if (_objSpawn.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float _directionX = Mathf.Clamp(player.directionLook.x * 2, -250, 250);
            float _directionY = Mathf.Clamp(player.directionLook.y * 2, -250, 250);

            rb.velocity += new Vector3(-_directionX, 0, -_directionY) * Time.deltaTime + Vector3.up * 4;
        }
    }
}
