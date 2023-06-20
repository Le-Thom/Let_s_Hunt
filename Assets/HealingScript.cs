using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealingScript : NetworkBehaviour
{
    [SerializeField] private GameObject animHealing;
    [ClientRpc]
    public void HealClientRpc(bool value)
    {
        animHealing.SetActive(value);
    }

}
