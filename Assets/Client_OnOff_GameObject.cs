using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Client_OnOff_GameObject : NetworkBehaviour
{
    private void OnEnable()
    {
        SetActiveClientRpc(true);
    }
    private void OnDisable()
    {
        SetActiveClientRpc(false);
    }
    [ClientRpc]
    private void SetActiveClientRpc(bool value)
    {
        gameObject.SetActive(value);
    }
}
