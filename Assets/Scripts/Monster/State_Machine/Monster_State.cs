using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_State
{
    public Monster_StateMachine stateMachine;
    public Monster_StateFactory factory;

    public Monster_State(Monster_StateMachine _stateMachine, Monster_StateFactory _factory)
    {
        stateMachine = _stateMachine;
        factory = _factory;
    }
    public virtual void EnterState()
    {

    }
    public virtual void UpdateState()
    {

    }
    public virtual void ExitState()
    {

    }
    public void SwitchState(Monster_State newState)
    {
        //We execute the end state and enter of the new State
        ExitState();
        Debug.Log("monster switch to" + newState.ToString());
        newState.EnterState();

        //We change the current state in the statemachine (brain)
        stateMachine.currentState = newState;
    }
}
