using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //========
    //MONOBEHAVIOUR
    //========
    private void Awake()
    {
        positionToMouse = PositionToMouse.Instance.transform;
    }
    private void Update()
    {
        transform.LookAt(positionToMouse);

        if(isSkillOnCooldown)
        {
            SkillRecharge();
        }
    }
    //========
    //FONCTION
    //========
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
