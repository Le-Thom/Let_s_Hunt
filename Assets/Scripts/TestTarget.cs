using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    public int hp;
    public void GetHit(int value)
    {
        hp -= value;
    }
}
