using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Monster_Manager : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    public Monster_Movement monster_Movement;
    public MonsterHitCollider monsterHitCollider;
    public NavMeshAgent navmesh => monster_Movement.navMeshAgent;
    [SerializeField] private Monster_Camera monster_Camera;
    private VisibleState currentState;

    [SerializeField] private float distanceBetweenMonsterAndSoldier = 10;

    private Vector3 positionOfTheMonster => monster_Movement.transform.position;

    //========
<<<<<<< Updated upstream
    //MONOBEHAVIOUR
    //========
    private void Update()
    {

    }
    //========
=======
>>>>>>> Stashed changes
    //FONCTIONS
    //========
    public void IsTheMonsterInFightState()
    {
        //await Task.Delay(1000);
        print(IsMonsterCloseToSoldier());
        if(IsMonsterCloseToSoldier())
        {
            SwitchVisibleState(VisibleState.OnlyVisibleByStreamer);
        }
        else
        {
            SwitchVisibleState(VisibleState.Invisible);
        }
    }
    public bool IsMonsterCloseToSoldier()
    {
        Ray ray = new Ray(Vector3.zero, Vector3.zero);
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(ray, distanceBetweenMonsterAndSoldier);
        foreach(RaycastHit sphereCastHit in sphereCastHits)
        {
            print(sphereCastHit.collider.name);
            if(sphereCastHit.collider.TryGetComponent<HunterHitCollider>(out HunterHitCollider hunterCollider))
            {
                return true;
            }
        }
        return false;
    }
    public void SwitchVisibleState(VisibleState newState)
    {
        currentState = newState;
        print(newState.ToString());
        switch(newState)
        {
            case VisibleState.OnlyVisibleByStreamer:
                monster_Camera.ChangeCameraState(MonsterCameraState.LockedCam);

                break;
            case VisibleState.Invisible:
                monster_Camera.ChangeCameraState(MonsterCameraState.FreeCam);
                break;
        }
    }
}
public enum VisibleState
{
    Invisible, OnlyVisibleByStreamer
}
