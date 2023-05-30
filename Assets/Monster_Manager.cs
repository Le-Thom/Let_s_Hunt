using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Monster_Manager : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    [SerializeField] private Monster_Movement monster_Movement;
    [SerializeField] private Monster_Camera monster_Camera;
    private VisibleState currentState;

    [SerializeField] private float distanceBetweenMonsterAndSoldier = 10;

    private Vector3 positionOfTheMonster => monster_Movement.transform.position;

    //========
    //MONOBEHAVIOUR
    //========

    //========
    //FONCTIONS
    //========
    public async void IsTheMonsterInFightState()
    {
        await Task.Delay(1000);
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
    }
}
public enum VisibleState
{
    Invisible, OnlyVisibleByStreamer
}
