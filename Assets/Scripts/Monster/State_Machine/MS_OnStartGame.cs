using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MS_OnStartGame : Monster_State
{
    public MS_OnStartGame(Monster_StateMachine _stateMachine, Monster_StateFactory _factory) : base(_stateMachine, _factory)
    {

    }
    public override async void EnterState()
    {
        stateMachine.monster_Camera.ChangeCameraState(MonsterCameraState.LockedCam);
        stateMachine.monster_Animator.SetTrigger("OnGameStarted");
        await Task.Delay(5000);
        stateMachine.monster_Input.enabled = true;
        SwitchState(factory.GetAnyState(MonsterState.Invisible));

        stateMachine.timeManager.StartTimeClientRpc();
    }
    public override void ExitState()
    {
        Debug.Log("Quiting Starting State");
    }
}
