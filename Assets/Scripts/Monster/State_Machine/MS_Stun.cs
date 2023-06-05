using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_Stun : Monster_State
{
    public MS_Stun(Monster_StateMachine _stateMachine, Monster_StateFactory _factory) : base(_stateMachine, _factory)
    {

    }
    public override void EnterState()
    {
        stateMachine.monster_Input.enabled = false;
        stateMachine.monster_Animator.SetTrigger("isStun");
    }
    public override void ExitState()
    {
        Debug.Log("Game is starting");
    }
}
