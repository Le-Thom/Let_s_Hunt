using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public bool open = false;
    public GameObject panel;

    public void Open()
    {
        open = !open;
        panel.SetActive(open);
    }


}
