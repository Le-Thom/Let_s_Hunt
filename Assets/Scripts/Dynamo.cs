using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FOW;
using NaughtyAttributes;
using Unity.Netcode;

public class Dynamo : NetworkBehaviour
{
    [SerializeField] private FogOfWarRevealer3D revealer3D;
    [SerializeField] private List<Light> lights;

    [SerializeField] private AnimationCurve curveDynamo;
    [SerializeField] private FMODUnity.EventReference LightOffAudio;

    public float timer = 0f;
    public float dynamoIntensity = 0f;
    public float rationTime = 10f;
    private float realRatioTime => 1 / rationTime;
    public float addingTime = 1f;

    private List<float> lightsIntensity = new List<float>();
    private float revealerLenght, revealerSmoothLenght, revealerAngle, revealerCloseLenght;

    [Header("BlackOut")]
    [SerializeField] private bool blackOut;
    [SerializeField] private float timerBlackOut;

    private void Start()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            lightsIntensity.Add(lights[i].intensity);
        }
        revealerLenght = revealer3D.ViewRadius;
        revealerSmoothLenght = revealer3D.AdditionalSoftenDistance;
        revealerAngle = revealer3D.ViewAngle;
        revealerCloseLenght = revealer3D.UnobscuredRadius;
    }

    private void Update()
    {
        timer -= Time.deltaTime * realRatioTime * Random.Range(0.8f, 1.2f);
        timer = Mathf.Clamp(timer, 0f, 1f);
        dynamoIntensity = curveDynamo.Evaluate(timer);

        if (blackOut && revealer3D.enabled == true)
        {
            revealer3D.enabled = false;
            FMODUnity.RuntimeManager.PlayOneShot(LightOffAudio, transform.position);
            return;
        }

        if (dynamoIntensity == 0f && revealer3D.enabled == true)
        {
            revealer3D.enabled = false;
            FMODUnity.RuntimeManager.PlayOneShot(LightOffAudio, transform.position);
        }
        else if (revealer3D.enabled == false && dynamoIntensity != 0f)
        {
            revealer3D.enabled = true;
        }

        revealer3D.ViewRadius = revealerLenght * dynamoIntensity;
        revealer3D.AdditionalSoftenDistance = revealerSmoothLenght * dynamoIntensity;
        revealer3D.ViewAngle = revealerAngle * dynamoIntensity;
        revealer3D.UnobscuredRadius = revealerCloseLenght * dynamoIntensity;

        for (int i = 0;i < lights.Count; i++)
        {
            lights[i].intensity = lightsIntensity[i] * dynamoIntensity;
        }

    }

    [ClientRpc]
    public void AddDynamoClientRpc()
    {
        timer += addingTime;
    }

    [ClientRpc]
    public void BlackOutClientRpc()
    {
        blackOut = true;
        StartCoroutine(BlackOutTimer());
    }

    public IEnumerator BlackOutTimer()
    {
        yield return new WaitForSeconds(timerBlackOut);
        blackOut = false;
    }
}
