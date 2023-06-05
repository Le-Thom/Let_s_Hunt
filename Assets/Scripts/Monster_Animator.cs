using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using DG.Tweening;

public class Monster_Animator : MonoBehaviour
{
    [SerializeField] private List<Animator> animatorsToSeedSpeed = new();
    [SerializeField] private Monster_StateMachine monster_Statemachine;
    [SerializeField] private float buffer = 0.2f;
    [SerializeField] private string alphaMaterialParameterName = "_Alpha";

    private List<SpriteRenderer> allTheSpriteRender = new();
    private List<Material> spritesMaterials = new();

    private NavMeshAgent navMesh => monster_Statemachine.monster_Movement.navMeshAgent;
    private void Awake()
    {
        allTheSpriteRender = gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach(SpriteRenderer spriteRenderer in allTheSpriteRender)
        {
            spritesMaterials.Add(spriteRenderer.material);
        }
    }
    private void Update()
    {
        float tktSpeed = navMesh.velocity.magnitude;

        foreach (Animator animatorMonster in animatorsToSeedSpeed)
        {
            animatorMonster.SetFloat("Speed", tktSpeed);
        }
    }
    [Button]
    public void SetInvisible()
    {
        foreach(Material material in spritesMaterials)
        {
            material.DOFloat(0, alphaMaterialParameterName, 3);
        }
    }
    [Button]
    public void SetVisible()
    {
        foreach (Material material in spritesMaterials)
        {
            material.DOFloat(1, alphaMaterialParameterName, 3);
        }
    }
}
