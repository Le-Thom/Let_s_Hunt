using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blackout_Competance_Monster : BaseCompetance_Monster
{
    [SerializeField] private float maxDistance = 10;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float accelerationBoost = 10;
    [SerializeField] private float speedBoost = 10;

    protected override async void SkillFonction()
    {
        await Task.Delay(timeBeforeTheAttack);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, maxDistance, Vector3.zero, playerLayer);
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterHitCollider))
            {
                hunterHitCollider.DeactivateFlashLightForXMillisecondSecond(timeOfTheAttack);
            }
        }
        monster_StateMachine.monsterHitCollider.GetMonsterInvincibleForXMiliseconds(timeOfTheAttack);

        float baseAcceleration = monster_StateMachine.Navmesh.acceleration;
        DOVirtual.Float(baseAcceleration, accelerationBoost, timeOfTheAttack / 1000, v => monster_StateMachine.Navmesh.acceleration = v)
            .OnComplete(() => DOVirtual.Float(accelerationBoost, baseAcceleration, 1, v => monster_StateMachine.Navmesh.acceleration = v));

        float baseSpeed = monster_StateMachine.Navmesh.speed;
        DOVirtual.Float(baseSpeed, speedBoost, timeOfTheAttack / 1000, v => monster_StateMachine.Navmesh.speed = v)
            .OnComplete(() => DOVirtual.Float(speedBoost, baseSpeed, 1, v => monster_StateMachine.Navmesh.speed = v));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
