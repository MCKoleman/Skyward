using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAfterEffect : MonoBehaviour
{
    public GameObject AfterVFX;
    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void OnParticleSystemStopped()
    {
        AfterVFX.GetComponent<ParticleSystem>().Play(true);
    }
}
