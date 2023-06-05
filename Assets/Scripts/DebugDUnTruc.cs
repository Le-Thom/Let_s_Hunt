using FOW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugDUnTruc : MonoBehaviour
{
    public FogOfWarWorld fog;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        fog.enabled = true;
    }
}
