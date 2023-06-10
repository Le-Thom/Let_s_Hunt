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
    private Tween onHitChange;
    private NavMeshAgent navMesh => monster_Statemachine.monster_Movement.navMeshAgent;
    private int numberOfParrelIsHitIsPlayed = 0;
    private void OnEnable()
    {
        MonsterHitCollider.onMonsterHit += HitMonsterFeedback;
    }
    private void OnDisable()
    {
        MonsterHitCollider.onMonsterHit -= HitMonsterFeedback;
    }
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
    [Button]
    public async void HitMonsterFeedback(int damage = 0)
    {
        numberOfParrelIsHitIsPlayed++;
        foreach (Material material in spritesMaterials)
        {
            material.SetFloat("_IsHit", 1);
        }

        await System.Threading.Tasks.Task.Delay(500);

        if (numberOfParrelIsHitIsPlayed <= 1)
        {
            foreach (Material material in spritesMaterials)
            {
                material.SetFloat("_IsHit", 0);
            }
        }
        numberOfParrelIsHitIsPlayed--;
    }
}
