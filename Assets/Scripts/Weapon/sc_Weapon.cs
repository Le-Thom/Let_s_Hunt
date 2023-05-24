using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon", menuName ="Game/Weapon")]
public class sc_Weapon : ScriptableObject
{
    public Sprite sprite;
    public float atkDistance;
    public GameObject go_Weapon_Hit_Prefab;

    public int DamageAtk1, DamageAtk2;
    public float cooldownAtk1, cooldownAtk2;
}
