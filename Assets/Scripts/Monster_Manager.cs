using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// The Ref Master
/// </summary>
public class Monster_Manager : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    public Monster_Movement monster_Movement;
    public MonsterHitCollider monsterHitCollider;
    public NavMeshAgent navmesh => monster_Movement.navMeshAgent;
    public Monster_Camera monster_Camera;
    public Component GetComponent(Component component)
    {
        return GetComponentInChildren(component.GetType());
    }
}
