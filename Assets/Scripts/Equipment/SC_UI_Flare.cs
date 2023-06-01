using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SC_UI_Flare : SC_UseItem
{
    [ServerRpc(RequireOwnership = false)]
    protected override void _UseItemServerRpc(Vector3 player, int equipment, Vector2 direction)
    {
        SC_sc_Object _sc_sc_equipment = Resources.Load<SC_sc_Object>("Equipment/");
        sc_Equipment _sc_equipment = _sc_sc_equipment.objects[equipment];

        GameObject _objSpawn = Instantiate(_sc_equipment.prefab_Object, player + Vector3.up * 1.5f, Quaternion.Euler(0, 0, 0));
        _objSpawn.GetComponent<NetworkObject>().Spawn(true);

        if (_objSpawn.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float _directionX = Mathf.Clamp(direction.x * 2, -250, 250);
            float _directionY = Mathf.Clamp(direction.y * 2, -250, 250);

            rb.velocity += new Vector3(-_directionX, 0, -_directionY) * Time.deltaTime + Vector3.up * 4;
        }
    }
}
