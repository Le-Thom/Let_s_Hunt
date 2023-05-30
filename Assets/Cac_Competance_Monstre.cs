using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cac_Competance_Monstre : BaseCompetance_Monster
{
    [SerializeField] private GameObject boxCollider;
    [SerializeField] private ParticleSystem cac_Particule;
    protected override async void SkillFonction()
    {
        cac_Particule.Play();
        await Task.Delay(timeBeforeTheAttack);

        boxCollider.SetActive(true);
        await Task.Delay(timeOfTheAttack);
        boxCollider.SetActive(false);
    }
}
