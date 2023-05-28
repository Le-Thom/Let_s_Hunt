using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Competence : MonoBehaviour
{
    public List<TestTarget> listTarget = new();
    public void Atk() => StartCoroutine(_Atk());
    protected virtual IEnumerator _Atk() { yield return null; Destroy(); }
    protected void Destroy() => Destroy(gameObject);
}
