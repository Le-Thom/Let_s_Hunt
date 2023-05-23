using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public virtual void IsClosestToInteract() { }
    public virtual void StopBeingTheClosest() { }
    public virtual void Interact() { }
}
