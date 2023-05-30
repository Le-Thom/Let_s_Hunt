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

    protected override async void SkillFonction()
    {
        await Task.Delay(timeBeforeTheAttack);
        collider.enabled = true;
        transform.DOMove(GetDashFinalPosition(), timeOfTheAttack).OnComplete(() => collider.enabled = false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterCollider))
        {
            //hunterCollider.Stun();
        }
    }
    private Vector3 GetDashFinalPosition()
    {
        NavMesh.SamplePosition(dashDestination.transform.position, out NavMeshHit hit, 999, NavMesh.AllAreas);
        return hit.position;
    }
}
