using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Competence", menuName = "Game/Monster competence")]
public class Sc_Competence : ScriptableObject
{
    public Sprite competenceSprite;
    public string competenceName;
    public int atkDamage;
    public float atkDistance;
    public float cooldown;

    public GameObject prefab_HitCollider;
}
