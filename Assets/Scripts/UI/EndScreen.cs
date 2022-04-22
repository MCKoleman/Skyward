using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject screenHolder;
    [SerializeField]
    private AudioSource endMusic;

    // Enables the hud. Passing false allows the same function to disable the hud
    public void EnableScreen(bool shouldEnable = true)
    {
        screenHolder.SetActive(shouldEnable);

        if (shouldEnable)
        {
            endMusic.Play();
        }
        else
        {
            endMusic.Stop();
            endMusic.time = 0.0f;
        }
    }
}
