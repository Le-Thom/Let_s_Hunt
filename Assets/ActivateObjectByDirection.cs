using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActivateObjectByDirection : MonoBehaviour
{
    [SerializeField] private GameObject up_Object;
    [SerializeField] private GameObject left_Object;
    [SerializeField] private GameObject right_Object;
    [SerializeField] private GameObject down_Object;

    [SerializeField] private bool isMonster = true;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private Tps_PlayerController tps_Controller;

    [SerializeField] private float buffer = 0.5f;

    private void Update()
    {
        Vector3 direction = Vector3.zero;

        if (isMonster)
        { direction = navMesh.desiredVelocity; }
        else
        { direction = tps_Controller.directionLook; }

        UpdateDirection(new (direction.x , direction.z));
    }
    public void UpdateDirection(Vector2 newDirection)
    {
        List<GameObject> objectToDisable = new() { up_Object, left_Object, right_Object, down_Object };
        bool isUpdated = false;

        if(Mathf.Abs(newDirection.y) > Mathf.Abs(newDirection.x))
        {
            print("Y is big");
            if (Mathf.Abs(newDirection.y) > buffer)
            {
                if (newDirection.y < 0)
                {
                    down_Object.SetActive(true);
                    objectToDisable.Remove(down_Object);
                    isUpdated = true;
                }
                if (newDirection.y > 0)
                {
                    up_Object.SetActive(true);
                    objectToDisable.Remove(up_Object);
                    isUpdated = true;
                }
            }

        }
        else
        {
            print("X is big");
            if(Mathf.Abs(newDirection.x) > buffer)
            {
                if(newDirection.x < 0)
                {
                    left_Object.SetActive(true);
                    objectToDisable.Remove(left_Object);
                    isUpdated = true;
                }
                if (newDirection.x > 0)
                {
                    right_Object.SetActive(true);
                    objectToDisable.Remove(right_Object);
                    isUpdated = true;
                }
            }
        }

        print(isUpdated);

        if(isUpdated) 
        {
            foreach (GameObject direction in objectToDisable)
            {
                print(direction);
                direction.SetActive(false);
            }
        }
    }
}
