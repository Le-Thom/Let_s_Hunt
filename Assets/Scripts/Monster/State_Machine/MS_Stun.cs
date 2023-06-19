using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_Stun : Monster_State
{
    private float navmeshOldSpeed = 0;
    public MS_Stun(Monster_StateMachine _stateMachine, Monster_StateFactory _factory) : base(_stateMachine, _factory)
    {

    }
    public override async void EnterState()
    {
        Debug.Log("Monster is Stun");
        stateMachine.monster_Input.enabled = false;
        stateMachine.Navmesh.destination = stateMachine.monster_Movement.transform.position;

        navmeshOldSpeed = stateMachine.Navmesh.speed;
        stateMachine.Navmesh.speed = 0;

        stateMachine.directional_Animator.isPositionLocked = true;
        stateMachine.directional_Animator.UpdateDirection(-DirectionToLook());

        await Task.Delay(stateMachine.stunTimeInMillisecond);
        SwitchState(factory.GetAnyState(MonsterState.Fight));
    }
    public override void ExitState()
    {
        stateMachine.monster_Input.enabled = true;
        stateMachine.Navmesh.speed = navmeshOldSpeed;
        stateMachine.directional_Animator.isPositionLocked = false;

        Debug.Log("Monster Is Un-Stun");
    }
    private Vector2 DirectionToLook()
    {
        Vector2 transformXZ = new Vector2 (stateMachine.monster_Movement.transform.position.x, stateMachine.monster_Movement.transform.position.z);
        Vector3 mousePosition = PositionToMouse.GetMouseWorldPosition(-1);

        Vector2 mouseDirection = transformXZ - new Vector2(mousePosition.x, mousePosition.z);
        mouseDirection = mouseDirection.normalized;
        return mouseDirection;
    }
}
