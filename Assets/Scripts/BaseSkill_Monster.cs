using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseCompetance_Monster : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Transform positionToMouse;
    [Header("In-Game ")]
    private bool isSkillOnCooldown = false;
    [SerializeField] private float cooldownMaxTimer = 10;
    private float cooldownCurrentTimer = 0;
    private Vector2 mousePositionOnViewport;
    private Camera _camera;
    private float _lookTargetRotation;

    //========
    //MONOBEHAVIOUR
    //========
    private void Awake()
    {
        positionToMouse = PositionToMouse.Instance.transform;
    }
    private void OnEnable()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        if(isSkillOnCooldown)
        {
            SkillRecharge();
        }
    }
    //========
    //FONCTION
    //========
    /// <summary>
    /// Input action of mouse position from player input in monster. (deliver a Vector2)
    /// </summary>
    /// <param name="ctx"></param>
    public void MousePosition(InputAction.CallbackContext ctx)
    {
        mousePositionOnViewport = ctx.ReadValue<Vector2>();
        SetDirectionMouse();
    }

    /// <summary>
    /// Set the rotation of the gameObject to the direction from him to mouse.
    /// </summary>
    private void SetDirectionMouse()
    {
        GetDirectionFromObjToMouse();
        Vector3 _direction = new Vector3(0, _lookTargetRotation, 0);
        transform.localRotation = Quaternion.Euler(_direction);
    }

    /// <summary>
    /// Set the lootTargetRotation used to make the Transform rotation of the gameObject.
    /// </summary>
    private void GetDirectionFromObjToMouse()
    {
        // target player to it's screen position.
        Vector3 objPosInViewport = _camera.WorldToScreenPoint(transform.position);

        // direction of the vector from player to mouse.
        Vector2 _direction = new Vector2(objPosInViewport.x - mousePositionOnViewport.x, objPosInViewport.y - mousePositionOnViewport.y);

        // calculate Y rotation from the direction.
        _lookTargetRotation = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg + 180f;
    }

    public void UseSkill()
    {
        //And If not stun
        if (isSkillOnCooldown) return;
        SkillFonction();
    }
    protected virtual void SkillFonction()
    {
        Debug.LogWarning("No Skill Register");
    }
    private void SkillRecharge()
    {
        if (cooldownCurrentTimer >= 100)
        {
            isSkillOnCooldown = false;
            cooldownCurrentTimer = 0;
        }
        else
            cooldownCurrentTimer += Time.deltaTime * cooldownMaxTimer / 100;
    }
}
