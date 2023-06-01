using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Animator : MonoBehaviour
{
    [SerializeField] private List<Animator> animatorsMonsterToSeedDirection = new();
    [SerializeField] private List<Animator> animatorsMonsterToSeedSpeed = new();
    [SerializeField] private Monster_Movement monster_Movement;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private float buffer = 0.2f;
    private void Update()
    {
        Vector3 directionMonster = monster_Movement.transform.position - navMesh.destination;
        
        foreach (Animator animatorMonster in animatorsMonsterToSeedDirection)
        {
                animatorMonster.SetFloat("XDirection", directionMonster.x);
                animatorMonster.SetFloat("YDirection", directionMonster.z);
        }

        float tktSpeed = navMesh.velocity.x + navMesh.velocity.y;

        foreach (Animator animatorMonster in animatorsMonsterToSeedSpeed)
        {
            animatorMonster.SetFloat("Speed", tktSpeed);
        }
    }
}
