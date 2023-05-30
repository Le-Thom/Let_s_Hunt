using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Monster_Camera : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    [Header("Free Camera Ref")]
    [SerializeField] private CinemachineVirtualCamera freeCamera;
    [SerializeField] private FollowGameObject freeCamFollowLockedScript;

    [Header("Locked Camera Ref")]
    [SerializeField] private CinemachineVirtualCamera lockedCamera;

    [Header("Free Camera Parameter")]
    [SerializeField] private float cameraWASDSpeed = 1.2f;
    [SerializeField] private float cameraMouseSpeed = 1.2f;
    [SerializeField] private float screenSizeThickness = 1.2f;
    [SerializeField] private bool canMoveCameraByMouse = false;
    private Vector3 movementDirectionCamera;

    private MonsterCameraState currentState = MonsterCameraState.FreeCam;

    //========
    //MONOBEHAVIOUR
    //========

    private void FixedUpdate()
    {
        //Camera Movement on Update So If Key is Long Press, continue to move
        freeCamera.transform.position += movementDirectionCamera * cameraWASDSpeed;
        OnCameraMovementBorderMouse();
    }

    //========
    //FONCTION
    //========
    public void OnCameraMovementWASD(InputAction.CallbackContext context)
    {
        if (!CanMoveCamera()) return;

        Vector3 movementDirection = context.ReadValue<Vector2>();

        movementDirectionCamera = new Vector3(movementDirection.x, 0, movementDirection.y);
    }
    public void OnCameraMovementBorderMouse()
    {
        if (!CanMoveCamera() || !canMoveCameraByMouse) return;

        Vector2 mousePositionOnScreen = Input.mousePosition;

        Vector3 newCameraPosition = freeCamera.transform.position;
        //This is boring

        //Up
        if(mousePositionOnScreen.x >= Screen.height - screenSizeThickness)
        {
            newCameraPosition.x += cameraMouseSpeed * Time.deltaTime;
        }

        //Down
        if (mousePositionOnScreen.x <= screenSizeThickness)
        {
            newCameraPosition.x -= cameraMouseSpeed * Time.deltaTime;
        }

        //Left
        if (mousePositionOnScreen.y >= Screen.height - screenSizeThickness)
        {
            newCameraPosition.z += cameraMouseSpeed * Time.deltaTime;
        }

        //Right
        if (mousePositionOnScreen.y <= screenSizeThickness)
        {
            newCameraPosition.z -= cameraMouseSpeed * Time.deltaTime;
        }

        freeCamera.transform.position = newCameraPosition;
    }
    public void ChangeCameraState(MonsterCameraState newState)
    {
        currentState = newState;
        switch(newState)
        {
            case MonsterCameraState.FreeCam:

                ChangeCameraPriority(10, 0);
                freeCamFollowLockedScript.enabled = false;
                break;
            case MonsterCameraState.LockedCam:

                ChangeCameraPriority(0, 10);
                freeCamFollowLockedScript.enabled = true;
                break;
        }
    }
    private bool CanMoveCamera()
    {
        return currentState == MonsterCameraState.FreeCam;
    }
    private void ChangeCameraPriority(int freeCam, int lockedCam)
    {
        freeCamera.Priority = freeCam;
        lockedCamera.Priority = lockedCam;
    }
}
public enum MonsterCameraState
{
    FreeCam, LockedCam
}
