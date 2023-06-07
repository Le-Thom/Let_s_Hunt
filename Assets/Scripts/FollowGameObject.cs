using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    //=========
    //VARIABLE
    //=========
    public Transform objectToFollow;
    public Vector3 offset;

    //=========
    //FONCTION
    //=========
    private void FixedUpdate()
    {
        transform.position = objectToFollow.position + offset;
    }
}
