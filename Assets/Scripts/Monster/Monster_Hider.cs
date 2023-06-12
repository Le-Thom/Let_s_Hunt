using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FOW;
using System.Linq;

/// <summary>
/// Set all the child sprite renderer to hid in the fog
/// </summary>
public class Monster_Hider : HiderBehavior
{
    public GameObject[] ObjectsToHide;
    public float alphaOnHide = 0;
    public float alphaOnReveal = 1;
    protected override void OnHide()
    {
        HideGameobjects();
    }

    protected override void OnReveal()
    {
        RevealGameObjects();
    }
    public void HideGameobjects()
    {
        foreach (GameObject objectToHide in ObjectsToHide)
        {
            foreach (SpriteRenderer o in objectToHide.GetComponentsInChildren<SpriteRenderer>())
                //Set the alpha To Hide the Monster
                o.color = new Vector4(o.color.r, o.color.g, o.color.b, alphaOnHide);
        }
    }
    public void RevealGameObjects()
    {
        foreach (GameObject objectToHide in ObjectsToHide)
        {
            foreach (SpriteRenderer o in objectToHide.GetComponentsInChildren<SpriteRenderer>())
                //Set the alpha To Reveal the monster
                o.color = new Vector4(o.color.r, o.color.g, o.color.b, alphaOnReveal);
        }
    }
    public void ModifyHiddenObjects(GameObject[] newObjectsToHide)
    {
        OnReveal();
        ObjectsToHide = newObjectsToHide;
        if (!enabled)
            return;

        if (!IsEnabled)
            OnHide();
        else
            OnReveal();
    }
}
