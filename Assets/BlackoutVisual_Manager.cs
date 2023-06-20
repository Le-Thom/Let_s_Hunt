using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BlackoutVisual_Manager : Singleton<BlackoutVisual_Manager>
{
    private void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    public async void ActivateBlackoutFeedback(int timeMilisecond)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        await Task.Delay(timeMilisecond);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
