using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Test : Weapon
{
    [SerializeField] private Transform collisionTransform;

    [SerializeField] private sc_Weapon weapon;
    private void Awake()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        collisionTransform.transform.localPosition = transform.right * weapon.atkDistance + transform.up * 0.5f + Vector3.forward * 1f;
    }
    protected override IEnumerator _Atk1()
    {
        yield return null;
        Debug.Log("ATK1 from Weapon_Test");
        for (int i = 0; i < listTarget.Count; i++)
        {
            listTarget[i].MonsterGetHitServerRpc(weapon.DamageAtk1);
        }

        Destroy();
    }

    protected override IEnumerator _Atk2()
    {
        yield return null;
        Debug.Log("ATK2 from Weapon_Test");
        for (int i = 0; i < listTarget.Count; i++)
        {
            listTarget[i].MonsterGetHitServerRpc(weapon.DamageAtk2);
        }

        Destroy();
    }

}
