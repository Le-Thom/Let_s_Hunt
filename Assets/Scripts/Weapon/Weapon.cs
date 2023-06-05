using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public List<MonsterHitCollider> listTarget = new();
    public void Atk1() => StartCoroutine(_Atk1());
    protected virtual IEnumerator _Atk1() { yield return null; Destroy(); }
    public void Atk2() => StartCoroutine(_Atk2());
    protected virtual IEnumerator _Atk2() { yield return null; Destroy(); }
    protected void Destroy() => Destroy(gameObject);
}
