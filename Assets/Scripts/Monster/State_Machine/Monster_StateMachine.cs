using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Monster_StateMachine : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    [Header("State Machine")]
    public Monster_State currentState;
    private Monster_StateFactory factory;
    public string nameOfTheCurrentState;

    [Header("Ref")]
    public Monster_Movement monster_Movement;
    public Monster_Camera monster_Camera;
    public PlayerInput monster_Input;
    public Animator monster_Animator;
    public MonsterHitCollider monsterHitCollider;
    public NavMeshAgent Navmesh => monster_Movement.navMeshAgent;

    //========
    //MONOBEHAVIOUR
    //========

    private void Start()
    {
        factory = new Monster_StateFactory(this);

        currentState = factory.GetAnyState(MonsterState.BeforeGame);
        currentState.EnterState();

        //
    }
    private void Update()
    {
        if(currentState != null)
        {
            currentState.UpdateState();
        }
    }

    //========
    //FONCTION
    //========
    public Component GetComponent(Component component)
    {
        return GetComponentInChildren(component.GetType());
    }
}
