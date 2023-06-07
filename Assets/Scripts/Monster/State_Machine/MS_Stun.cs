using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_Stun : Monster_State
{
    public MS_Stun(Monster_StateMachine _stateMachine, Monster_StateFactory _factory) : base(_stateMachine, _factory)
    {

    }
    public override async void EnterState()
    {
        Debug.Log("Monster is Stun");
        stateMachine.monster_Input.enabled = false;
        stateMachine.monster_Animator.SetTrigger("isStun");

        await Task.Delay(stateMachine.stunTimeInMillisecond);
        SwitchState(factory.GetAnyState(MonsterState.Fight));
    }
    public override void ExitState()
    {
        Debug.Log("Monster Is Un-Stun");
    }
}
