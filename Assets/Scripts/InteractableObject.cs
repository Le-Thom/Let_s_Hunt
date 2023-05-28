using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected bool isInteractable;

    public virtual void IsClosestToInteract() { }
    public virtual void StopBeingTheClosest() { }
    public virtual void Interact() { }
    public virtual void SetInteractable(bool value) { }
    public bool IsInteractable() { return isInteractable; }
}
