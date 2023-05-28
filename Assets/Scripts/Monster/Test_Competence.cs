using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Competence : Competence
{
    [SerializeField] private Transform collisionTransform;
    [SerializeField] private Sc_Competence competence;
    private void Awake()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        collisionTransform.transform.localPosition = transform.right * competence.atkDistance + transform.up * 0.5f + Vector3.forward * 1f;
    }
    protected override IEnumerator _Atk()
    {
        yield return null;
        Debug.Log("ATK1 from Weapon_Test");
        for (int i = 0; i < listTarget.Count; i++)
        {
            listTarget[i].GetHit(competence.atkDamage);
        }

        Destroy();
    }
}
