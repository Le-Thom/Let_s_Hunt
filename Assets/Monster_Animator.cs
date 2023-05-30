using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Animator : MonoBehaviour
{
    [SerializeField] private Animator animatorMonster;
    [SerializeField] private Monster_Movement monster_Movement;
    [SerializeField] private NavMeshAgent navMesh;
    private void Update()
    {
        Vector3 vector3 = monster_Movement.transform.position - navMesh.destination;
        animatorMonster.SetFloat("", vector3.x);
        animatorMonster.SetFloat("", vector3.z);
    }
}
