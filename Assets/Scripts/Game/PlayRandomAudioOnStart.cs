using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomAudioOnStart : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
