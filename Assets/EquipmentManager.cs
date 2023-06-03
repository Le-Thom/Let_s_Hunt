using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class EquipmentManager : NetworkBehaviour
{
    [SerializeField] private SC_sc_Object _sc_sc_equipment;
    [SerializeField] private sc_Object flare;
    [SerializeField] private sc_Object medkit;
    [SerializeField] private sc_Object trap;

    public void UseItem(Tps_PlayerController player, sc_Equipment _equipment, Equipment _equip)
    {
        if (_equipment.type == equipmentType.MEDKIT && player.GetCurrentState() == AkarisuMD.Player.StateId.HEALING)
        {
            Debug.Log("Bloque healing");
            return;
        }


        switch (_equipment.type)
        {
            case equipmentType.FLARE:
                _UseFlareServerRpc(player.transform.position, player.directionLook);

                break;

            case equipmentType.TRAP:
                _UseTrapServerRpc(player.transform.position, player.directionLook);

                break;

            case equipmentType.MEDKIT:
                _UseMedKit(player);

                break;
        }
        _equip.ItemUsed();

    }

    [ServerRpc(RequireOwnership = false)]
    protected void _UseFlareServerRpc(Vector3 player, Vector2 direction)
    {
        sc_Equipment _sc_equipment = flare as sc_Equipment;

        GameObject _objSpawn = Instantiate(_sc_equipment.prefab_Object, player + Vector3.up * 1.5f, Quaternion.Euler(0, 0, 0));
        _objSpawn.GetComponent<NetworkObject>().Spawn(true);

        if (_objSpawn.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float _directionX = Mathf.Clamp(direction.x * 2, -250, 250);
            float _directionY = Mathf.Clamp(direction.y * 2, -250, 250);

            rb.velocity += new Vector3(-_directionX, 0, -_directionY) * Time.deltaTime + Vector3.up * 4;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    protected void _UseTrapServerRpc(Vector3 player, Vector2 direction)
    {
        sc_Equipment _sc_equipment = trap as sc_Equipment;

        GameObject _objSpawn = Instantiate(_sc_equipment.prefab_Object, player + Vector3.up * 1.5f, Quaternion.Euler(0, 0, 0));
        _objSpawn.GetComponent<NetworkObject>().Spawn(true);

        if (_objSpawn.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float _directionX = Mathf.Clamp(direction.x * 2, -250, 250);
            float _directionY = Mathf.Clamp(direction.y * 2, -250, 250);

            rb.velocity += new Vector3(-_directionX, 0, -_directionY) * Time.deltaTime + Vector3.up * 4;
        }
    }

    protected void _UseMedKit(Tps_PlayerController player)
    {
        player.ChangeStateToPlayerHealing();
    }
}

public enum equipmentType
{
    FLARE,
    TRAP,
    MEDKIT,
}