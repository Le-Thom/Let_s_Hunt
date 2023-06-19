using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.InputSystem;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;

public class Monster_Skills : MonoBehaviour
{
    //========
    //VARIABLES
    //========
    [SerializeField] private Monster_StateMachine monster_StateMachine;
    [SerializeField] private List<BaseCompetance_Monster> listOfCompetance = new();
    [SerializeField] private AnimationSequencerController skillUIAnimationController;
    public static Action<int, AttackAnim> whenASkillIsUsed;

    //========
    //MONOBEHAVIOUR
    //========

    private void Update()
    {
        if (IsPlayerInStateFighting()) return;
        if(AllCooldownAreFinished())
        {
            DeactivateSkillUI();
        }
    }

    private bool IsPlayerInStateFighting()
    {
        return monster_StateMachine.isInFightState;
    }

    private bool AllCooldownAreFinished()
    {
        bool value = true;
        foreach (BaseCompetance_Monster skill in listOfCompetance)
        {
            if (skill.isSkillOnCooldown)
            {
                value = false;
            }
        }
        return value;
    }

    //========
    //FONCTION
    //========
    public void InputMousePosition(InputAction.CallbackContext context)
    {
        foreach(BaseCompetance_Monster competance_Monster in listOfCompetance)
        {
            competance_Monster.MousePosition(context);
        }
    }
    public void CanMonsterUseSkill(bool value)
    {
        if(value)
        {
            ActivateSkill();
            //ActivateSkillUI();
        }
        else
        {
            DeactivateAllSkill();
            //DeactivateSkillUI();
        }
    }
    private void ActivateSkill()
    {   
        if (listOfCompetance[0].canUsedSkill) return;
        foreach (BaseCompetance_Monster skill in listOfCompetance)
        {
            skill.canUsedSkill = true;
        }
        ActivateSkillUI();
    }
    private void DeactivateAllSkill()
    {
        //if (!listOfCompetance[0].enabled) return;
        foreach (BaseCompetance_Monster skill in listOfCompetance)
        {
            skill.canUsedSkill = false;
        }
    }
    private void ActivateSkillUI()
    {
        //skillUIAnimationController.SetProgress(0);
        skillUIAnimationController.transform.DOScale(1, 1);
        skillUIAnimationController.PlayForward();
    }
    private void DeactivateSkillUI()
    {
        //skillUIAnimationController.SetProgress(1);
        skillUIAnimationController.transform.DOScale(Vector3.zero, 1);
    }
}
public enum AttackAnim
{
    Bite, Dash
}
