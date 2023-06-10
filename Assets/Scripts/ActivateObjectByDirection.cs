using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class ActivateObjectByDirection : NetworkBehaviour
{
    [SerializeField] private GameObject up_Object;
    [SerializeField] private GameObject left_Object;
    [SerializeField] private GameObject right_Object;
    [SerializeField] private GameObject down_Object;

    [SerializeField] private bool isMonster = true;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private Tps_PlayerController tps_Controller;

    [SerializeField] private float buffer = 0.5f;
    [SerializeField] private bool debug = false;

    private void Update()
    {
        if (!IsOwner && !debug) return;
        Vector3 direction;

        if (isMonster)
        { direction = navMesh.desiredVelocity; 

        UpdateDirection(new(direction.x, direction.z));
        }
    }

    public void UpdateDirection(Vector2 newDirection)
    {
        List<GameObject> objectToDisable = new() { up_Object, left_Object, right_Object, down_Object };
        bool isUpdated = false;
        GameObject directionToActivate = null;
        float absY = Mathf.Abs(newDirection.y);
        float absX = Mathf.Abs(newDirection.x);

        if (debug) print(absY.ToString()+ " : " + absX.ToString() + "= Direction");
        if (absY > absX)
        {
            if(debug) print("Y is big");
            if (Mathf.Abs(newDirection.y) > buffer)
            {
                if (newDirection.y < 0)
                {
                    directionToActivate = down_Object;
                    objectToDisable.Remove(down_Object);
                    isUpdated = true;
                }
                if (newDirection.y > 0)
                {
                    directionToActivate = up_Object;
                    objectToDisable.Remove(up_Object);
                    isUpdated = true;
                }
            }

        }
        else
        {
            if (debug) print("X is big");
            if(Mathf.Abs(newDirection.x) > buffer)
            {
                if(newDirection.x < 0)
                {
                    directionToActivate = left_Object;
                    objectToDisable.Remove(left_Object);
                    isUpdated = true;
                }
                if (newDirection.x > 0)
                {
                    directionToActivate = right_Object;
                    objectToDisable.Remove(right_Object);
                    isUpdated = true;
                }
            }
        }

        if(isUpdated) 
        {
            foreach (GameObject direction in objectToDisable)
            {
                direction.SetActive(false);
            }
            directionToActivate.SetActive(true);
        }
    }
}
