using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetenceAtkCollider : MonoBehaviour
{
    [SerializeField] private Competence target;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TestTarget>(out TestTarget component))
        {
            target.listTarget.Add(component);
        }
    }
}
