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
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, playerLayer);
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterHitCollider))
            {
                hunterHitCollider.DeactivateFlashLightForXMillisecondSecond(timeOfTheAttack);
            }
        }
        monster_StateMachine.monsterHitCollider.GetMonsterInvincibleForXMiliseconds(timeOfTheAttack);

        /*
        float baseAcceleration = monster_StateMachine.Navmesh.acceleration;
        DOVirtual.Float(baseAcceleration, accelerationBoost, timeOfTheAttack / 1000 * 0.5f, v => monster_StateMachine.Navmesh.acceleration = v)
            .OnComplete(() => DOVirtual.Float(accelerationBoost, baseAcceleration, timeOfTheAttack / 1000 * 0.5f, v => monster_StateMachine.Navmesh.acceleration = v));

        float baseSpeed = monster_StateMachine.Navmesh.speed;
        DOVirtual.Float(baseSpeed, speedBoost, timeOfTheAttack / 1000 * 0.5f, v => monster_StateMachine.Navmesh.speed = v)
            .OnComplete(() => DOVirtual.Float(speedBoost, baseSpeed, timeOfTheAttack / 1000 * 0.5f, v => monster_StateMachine.Navmesh.speed = v));
        */
        monster_StateMachine.monster_Movement.ChangeSpeed(MonsterSpeed.Blackout);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
