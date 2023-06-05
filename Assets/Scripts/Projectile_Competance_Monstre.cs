using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile_Competance_Monstre : BaseCompetance_Monster
{
    protected override async void SkillFonction()
    {
        await Task.Delay(timeBeforeTheAttack);
    }
}
