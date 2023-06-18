using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.InputSystem;
using BrunoMikoski.AnimationSequencer;

public class Monster_Skills : MonoBehaviour
{
    //========
    //VARIABLES
    //========
    [SerializeField] private List<BaseCompetance_Monster> listOfCompetance = new();
    [SerializeField] private AnimationSequencerController skillUIAnimationController;
    public static Action<int, AttackAnim> whenASkillIsUsed;

    //========
    //MONOBEHAVIOUR
    //========

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
    {   if (listOfCompetance[0].enabled) return;
        foreach (BaseCompetance_Monster skill in listOfCompetance)
        {
            skill.isSkillOnCooldown = true;
            skill.CooldownTimer = 0;
            skill.enabled = true;
        }
        ActivateSkillUI();
    }
    private void DeactivateAllSkill()
    {
        //if (!listOfCompetance[0].enabled) return;
        foreach (BaseCompetance_Monster skill in listOfCompetance)
        {
            skill.enabled = false;
        }
        DeactivateSkillUI();
    }
    private void ActivateSkillUI()
    {
        //skillUIAnimationController.SetProgress(0);
        skillUIAnimationController.PlayForward();
    }
    private void DeactivateSkillUI()
    {
        //skillUIAnimationController.SetProgress(1);
        skillUIAnimationController.PlayBackwards();
    }
}
public enum AttackAnim
{
    Bite, Dash
}
