using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip[] clips;

    // Plays a random clip in the current source
    public void PlayRandomClip()
    {
        // If there is no source, get the SFX channel from AudioManager
        if (source == null)
            source = AudioManager.Instance.GetSFXSource();

        // Play the clip in the channel
        source.clip = clips[Random.Range(0,clips.Length)];
        source.Play();
    }
}
