using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cac_Competance_Monstre : BaseCompetance_Monster
{
    [SerializeField] private new GameObject collider;
    [SerializeField] private ParticleSystem cac_particule;

    protected override async void SkillFonction()
    {
        isAttacking = true;
        cac_particule.Play();

        collider.SetActive(true);

        await Task.Delay(timeOfTheAttack);

        collider.SetActive(false);
        isAttacking = false;
    }
}
