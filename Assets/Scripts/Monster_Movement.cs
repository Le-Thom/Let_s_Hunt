using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Monster_Movement : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    [Header("Ref")]
    [SerializeField] private ParticleSystem onClickParticule;
    [SerializeField] private GameObject mousePointeur;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Movement Variable")]
    [SerializeField] private float monsterSpeed = 2;
    [SerializeField] private float cameraSpeed = 1.2f;

    [SerializeField] private LayerMask cameraCastLayer;
    private Vector3 movementDirectionCamera;
    //========
    //MONOBEHAVIOUR
    //========
    private void FixedUpdate()
    {
        //Camera Movement on Update So If Key is Long Press, continue to move
        virtualCamera.transform.position += movementDirectionCamera * cameraSpeed;
    }

    //========
    //FONCTION
    //========
    public void OnClickMovement(InputAction.CallbackContext context = new ())
    {
        Vector3 newDestination = GetMouseWorldPosition(cameraCastLayer);
        Debug.Log("Monster Click Detected / Position = " + newDestination);
        if (newDestination == Vector3.zero) return;

        //Feedback
        mousePointeur.transform.position = newDestination;
        onClickParticule.Play();
    }
    public void OnCameraMovement(InputAction.CallbackContext context)
    {
        Vector3 movementDirection = context.ReadValue<Vector2>();

        movementDirectionCamera = new Vector3(movementDirection.x, 0, movementDirection.y);
    }
    /// <summary>
    /// Get The Closer Navmesh Position to the world
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    public static Vector3 GetMouseWorldPosition(LayerMask layerMask)
    {
        Vector3 positionMouse = PositionToMouse.GetMouseWorldPosition(layerMask);
        //Nav mesh things
        return positionMouse;
    }
}
