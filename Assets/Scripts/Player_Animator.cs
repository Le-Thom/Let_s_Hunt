using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using DG.Tweening;

public class Player_Animator : NetworkBehaviour
{
    [SerializeField] private List<Animator> animatorToSendSpeed;
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
        MonsterHitCollider.onMonsterHit += HitFeedback;

    }
    private void OnDisable()
    {
        MonsterHitCollider.onMonsterHit -= HitFeedback;
    }
    private void Awake()
    {
        allTheSpriteRender = gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach(SpriteRenderer spriteRenderer in allTheSpriteRender)
        {
            spritesMaterials.Add(spriteRenderer.material);
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
    public async void HitFeedback(int damage = 0)
    {
        numberOfParrelIsHitIsPlayed++;

        IsHitFeedbackClientRpc(true);

        await System.Threading.Tasks.Task.Delay(500);

        if (numberOfParrelIsHitIsPlayed <= 1)
        {
            IsHitFeedbackClientRpc(false);
        }
        numberOfParrelIsHitIsPlayed--;
    }
    [ClientRpc]
    public void IsHitFeedbackClientRpc(bool value)
    {
        if(value)
        {
            foreach (Material material in spritesMaterials)
            {
                material.SetFloat("_IsHit", 1);
            }
        }
        else
        {
            foreach (Material material in spritesMaterials)
            {
                material.SetFloat("_IsHit", 0);
            }
        }
    }
    public void SendSpeedToAnimator(float speed)
    {
        speed = Mathf.Clamp(speed, 0, 1);
        foreach (Animator animator in animatorToSendSpeed)
        {
            animator.SetFloat("speed", speed);
        }
    }
    public void AttackAnimator()
    {
        foreach (Animator animator in animatorToSendSpeed)
        {
            animator.SetTrigger("whenAttack");
        }
    }
    public void DashAnimator()
    {
        foreach (Animator animator in animatorToSendSpeed)
        {
            animator.SetTrigger("whenDash");
        }
    }
    public Animator GetPlayerAnimator(int index)
    {
        if (animatorToSendSpeed.Count > index)
        {
            return animatorToSendSpeed[index];
        }
        else return animatorToSendSpeed[0];
    }
}
