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
        stateMachine.monster_Camera.ChangeCameraState(MonsterCameraState.FreeCam);
        stateMachine.monster_Movement.ChangeSpeed(MonsterSpeed.Invisible, true);
        stateMachine.monster_Skills.CanMonsterUseSkill(false);
        stateMachine.monster_Hider.alphaOnHide = stateMachine.alphaOnInvisible;
        stateMachine.monster_Hider.RefreshHide();
        stateMachine.isInFightState = false;

        stateMachine.onUpdate += UpdateState;
    }
    public override void UpdateState()
    {
        if(stateMachine.IsMonsterCloseToHunter(stateMachine.maxDistanceForEnteringInFight))
        {
            SwitchState(factory.GetAnyState(MonsterState.Fight));
        }
    }
    public override void ExitState()
    {
        stateMachine.onUpdate -= UpdateState;
        Debug.Log("Exiting Invisiblity");
    }
}
