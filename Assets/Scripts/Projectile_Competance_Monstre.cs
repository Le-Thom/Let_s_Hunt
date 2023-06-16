using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;


public class Projectile_Competance_Monstre : BaseCompetance_Monster
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject projectileAOE;
    protected override async void SkillFonction()
    {
        isAttacking = true;
        
        GameObject AOE = Instantiate(projectileAOE);
        AOE.transform.position = PositionToMouse.GetMouseWorldPosition(groundLayer);
        AOE.GetComponent<NetworkObject>().Spawn(true);

        await Task.Delay(timeOfTheAttack);

        //AOE.GetComponent<NetworkObject>().Despawn();
        Destroy(AOE);

        isAttacking = false;
    }
}
