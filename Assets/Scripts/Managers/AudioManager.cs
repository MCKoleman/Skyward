using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Sound Sources")]
    [SerializeField]
    private AudioSource mainMusicSource;
    [SerializeField]
    private AudioSource transitionMusicSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource tempSource;

    [Header("Sound clips")]
    [SerializeField]
    private AudioClip bossIntro;
    [SerializeField]
    private AudioClip bossMusic;
    [SerializeField]
    private AudioClip mainMenuIntro;
    [SerializeField]
    private AudioClip mainMenuMusic;
    [SerializeField]
    private List<AudioClip> sceneMusic;
    [SerializeField]
    private AudioClip uiSelect;
    [SerializeField]
    private AudioClip uiSubmit;
    [SerializeField]
    private AudioClip[] musicList;

    // Info
    private bool isLooping;
    private const float AUDIO_INTERVAL = 3.0f;
    private IEnumerator<AudioClip> currTrack;

    // Initializes the audio manager
    public void Init()
    {
        //StopMusic();
        currTrack = sceneMusic.GetEnumerator();
    }

    // Stops the currently playing music and resets the manager
    public void StopMusic()
    {
        Debug.Log("Stop music called");
        mainMusicSource.Stop();
        transitionMusicSource.Stop();
        isLooping = false;
    }

    // Plays the UI select effect
    public void PlayUISelect()
    {
        tempSource.clip = uiSelect;
        tempSource.Play();
    }

    // Plays the UI submit effect
    public void PlayUISubmit()
    {
        tempSource.clip = uiSubmit;
        tempSource.Play();
    }

    // Plays the Boss Intro music
    public void PlayBossIntro()
    {
        tempSource.clip = bossIntro;
        tempSource.Play();
    }

    // Plays the Boss Fight music
    public void PlayBossFight()
    {
        tempSource.clip = bossMusic;
        tempSource.Play();
    }

    // Plays the next clip
    public void HandleSceneChange()
    {
        if (currTrack.MoveNext())
        {
            tempSource.clip = currTrack.Current;
        }

        tempSource.Play();
    }

    // Plays the given clip in the temporary channel
    public void PlayClip(AudioClip clip)
    {
        tempSource.clip = clip;
        tempSource.Play();
    }

    // Plays the given music clip
    public void PlayMusicClip(AudioClip clip)
    {
        mainMusicSource.clip = clip;
        mainMusicSource.Play();
    }

    // Returns the SFX source
    public AudioSource GetSFXSource() { return sfxSource; }
}
