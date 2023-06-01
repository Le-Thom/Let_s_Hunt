using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class InteractableObject : NetworkBehaviour
{
    protected bool isInteractable;

    public virtual void IsClosestToInteract() { }
    public virtual void StopBeingTheClosest() { }

    [ClientRpc]
    public virtual void InteractClientRpc() { }
    [ServerRpc]
    public virtual void InteractServerRpc() { }
    public virtual void SetInteractable(bool value) { }
    public bool IsInteractable() { return isInteractable; }
}
