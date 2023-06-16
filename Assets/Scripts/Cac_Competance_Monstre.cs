using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class Cac_Competance_Monstre : BaseCompetance_Monster
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool debug = false;
    protected override async void SkillFonction()
    {
        isAttacking = true;

        animator.GetComponent<ClientNetworkAnimator>().SetTrigger("OnAttack");
        if(debug) animator.SetTrigger("OnAttack");

        await Task.Delay(timeOfTheAttack);

        isAttacking = false;
    }
}
