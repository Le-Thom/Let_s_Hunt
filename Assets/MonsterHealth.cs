using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.Netcode;
using System;

public class MonsterHealth : MonoBehaviour
{
    //========
    //VARIABLES
    //========

    public static event Action whenTheMonsterDied;

    [SerializeField] private Slider hpBar;
    [SerializeField] private int maxMonsterHp = 1000; 

    private int monsterHP = 1000;

    //========
    //MONOBEHAVIOUR
    //========

    private void OnEnable()
    {
        MonsterHitCollider.onMonsterHit += UpdateHpToDamageOrHeal;


        monsterHP = maxMonsterHp;
        Debug.Log("Init Monster HP");
        
        hpBar.maxValue = monsterHP;
    }

    private void OnDisable()
    {
        MonsterHitCollider.onMonsterHit -= UpdateHpToDamageOrHeal;
    }

    //========
    //FONCTION
    //========

    /// <summary>
    /// Update The Hp Bar And Variable
    /// </summary>
    /// <param name="Damage"></param>
    public void UpdateHpToDamageOrHeal(int damageOrHeal)
    {
        /*if (!IsHost) 
        {
            Debug.LogError("Client trying to change health Monster");
            return; 
        }*/

        monsterHP += damageOrHeal;

        hpBar.value = monsterHP;

        Debug.Log("Monster is Damage");

        if (IsTheMonsterDead() && whenTheMonsterDied!= null)
        {
            whenTheMonsterDied();
        }
    }
    private bool IsTheMonsterDead()
    {
        return monsterHP <= 0;
    }
}
