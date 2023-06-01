using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class Dash_Competance_Monster : BaseCompetance_Monster
{
    [SerializeField] private new Collider collider;
    [SerializeField] private GameObject dashDestination;
    [SerializeField] private float dashForce;

    private NavMeshAgent Monster_Navmesh => monster_StateMachine.Navmesh;
    //private Monster_Movement Monster_Movement => monster_StateMachine.monster_Movement;

    protected override async void SkillFonction()
    {
        await Task.Delay(timeBeforeTheAttack);

        if (NavMesh.SamplePosition(dashDestination.transform.position, out NavMeshHit hit, 999f, -1))
        {
            print(hit.position);
            collider.enabled = true;
            Monster_Navmesh.destination = hit.position;
            OnEndDash();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterCollider))
        {
            hunterCollider.StunHunter();
        }
    }
    private void OnEndDash()
    {
        collider.enabled = false;
    }
}
