using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float timeToArm;

    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private Animator _animator;

    private int _animActivate;

    private void Start()
    {
        SetANimIndex();
        StartCoroutine(waitFor());
    }

    private void SetANimIndex()
    {
        _animActivate = Animator.StringToHash("Activate");
    }

    IEnumerator waitFor()
    {
        yield return new WaitForSeconds(timeToArm);
        _sphereCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if !isHost return;

        if (other.TryGetComponent<MonsterHitCollider>(out MonsterHitCollider mhc))
        {
            _animator.SetTrigger(_animActivate);
            mhc.GetStun();
        }
    }
}
