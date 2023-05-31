using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cac_Competance_Monstre : BaseCompetance_Monster
{
    [SerializeField] private Animator cac_Animation;
    [SerializeField] private string animationName;
    protected override async void SkillFonction()
    {
        Transform currentParent = transform;
        cac_Animation.transform.SetParent(monster_Manager.transform);

        cac_Animation.Play(animationName);
        await Task.Delay(timeOfTheAttack);

        cac_Animation.transform.SetParent(currentParent);
    }
}
