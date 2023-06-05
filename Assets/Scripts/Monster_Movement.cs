using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.AI;

public class Monster_Movement : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    [Header("Ref")]
    [SerializeField] private ParticleSystem onClickParticule;
    [SerializeField] private GameObject mousePointeur;
    public NavMeshAgent navMeshAgent;

    [Header("Movement Variable")]
    [SerializeField] private float monsterSpeed = 2;

    [SerializeField] private LayerMask cameraCastLayer;

    //========
    //FONCTION
    //========
    public void OnClickMovement(InputAction.CallbackContext context = new ())
    {
        Vector3 newDestination = GetMouseWorldPositionOnNavmesh(cameraCastLayer);
        Debug.Log("Monster Click Detected / Position = " + newDestination);
        if (newDestination == Vector3.zero) return;

        //Feedback
        mousePointeur.transform.position = newDestination;
        onClickParticule.Play();

        navMeshAgent.SetDestination(newDestination);
    }
    /// <summary>
    /// Get The Closer Navmesh Position to the world
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public static Vector3 GetMouseWorldPositionOnNavmesh(LayerMask layerMask)
    {
        Vector3 positionMouse = PositionToMouse.GetMouseWorldPosition(layerMask);
        //Nav mesh things

        Vector3 positionOnNavmesh = positionMouse;

        /*if (NavMesh.SamplePosition(positionMouse.normalized, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {
            positionOnNavmesh = hit.position;
        }*/

        return positionOnNavmesh;
    }
}
