using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_Fight : Monster_State
{
    public MS_Fight(Monster_StateMachine _stateMachine, Monster_StateFactory _factory) : base(_stateMachine, _factory)
    {

    }
    public override void EnterState()
    {

    }
    public override void ExitState()
    {
        Debug.Log("Game is starting");
    }
}
