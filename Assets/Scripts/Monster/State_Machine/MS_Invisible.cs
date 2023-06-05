using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_Invisible : Monster_State
{
    public MS_Invisible(Monster_StateMachine _stateMachine, Monster_StateFactory _factory) : base(_stateMachine, _factory)
    {

    }
    public override void EnterState()
    {
        stateMachine.monster_Animator.SetBool("isVisible", false);
    }
    public override void ExitState()
    {
        Debug.Log("Exiting Invisiblity");
    }
}
