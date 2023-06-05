using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_BeforeGame : Monster_State
{
    public MS_BeforeGame(Monster_StateMachine _stateMachine, Monster_StateFactory _factory) : base(_stateMachine, _factory)
    {

    }
    public override void EnterState()
    {
        stateMachine.monster_Input.enabled = false;
    }
    public override void ExitState()
    {
        Debug.Log("Game is starting");
    }
}
