using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionToMouse : Singleton<PositionToMouse>
{
    [SerializeField] private LayerMask layerMask;
    //========
    //MONOBEHAVIOUR
    //========

    void Update()
    {
        transform.position = GetMouseWorldPosition(layerMask);
    }
    //========
    //FONCTION
    //========
    public static Vector3 GetMouseWorldPosition(LayerMask layerMask)
    {
        //using old Inputs System
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layerMask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
