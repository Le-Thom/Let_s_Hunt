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
    private bool isDashStarted = false;

    protected override async void SkillFonction()
    {
        await Task.Delay(timeBeforeTheAttack);

        isDashStarted = true;
        collider.enabled = true;
        isAttacking = true;
        /*if(!Monster_Navmesh.Warp(transform.position + transform.forward * dashForce))
        {
            print("warp failed");
            Monster_Navmesh.Warp(dashDestination.transform.position);
        }*/

        await Task.Delay(timeOfTheAttack);


        isDashStarted = false;
        collider.enabled = false;
        isAttacking = false;
        Monster_Navmesh.SetDestination(Monster_Navmesh.transform.position);
        /*
        if (NavMesh.SamplePosition(dashDestination.transform.position, out NavMeshHit hit, 999f, -1))
        {
            print(hit.position);
            collider.enabled = true;
            Monster_Navmesh.destination = hit.position;
        }*/
    }
    private void FixedUpdate()
    {
        if (isDashStarted)
            Monster_Navmesh.Move(transform.forward * dashForce);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterCollider))
        {
            hunterCollider.StunHunter();
        }
    }
    private Vector3 GetDashDirection()
    {
        return transform.position - dashDestination.transform.position;
    }
}
