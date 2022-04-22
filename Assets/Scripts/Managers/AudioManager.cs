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

    private GlobalVars.DungeonTheme prevTheme = GlobalVars.DungeonTheme.SKY;

    // Info
    private bool isLooping;
    private const float AUDIO_INTERVAL = 3.0f;
    private IEnumerator<AudioClip> currTrack;

    // Initializes the audio manager
    public void Init()
    {
        //StopMusic();
        currTrack = sceneMusic.GetEnumerator();
        mainMusicSource.loop = true;
    }

    // Stops the currently playing music and resets the manager
    public void StopMusic()
    {
        Debug.Log("Stop music called");
        mainMusicSource.Stop();
        transitionMusicSource.Stop();
        isLooping = false;
    }

    //BUG: CAUSE INFINITE-LOOP CLICKING NOISE UNTIL SCENE CHANGE

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
        mainMusicSource.clip = bossIntro;
        mainMusicSource.Play();
    }

    // Plays the Boss Fight music
    public void PlayBossFight()
    {
        mainMusicSource.clip = bossMusic;
        mainMusicSource.Play();
    }

    public void PlayMainMenu()
    {
        mainMusicSource.clip = musicList[0];
        mainMusicSource.Play();
        prevTheme = GlobalVars.DungeonTheme.DEFAULT;
    }

    // Plays the next clip
    public void HandleSceneChange(LevelProgressList.LevelProgress curLevel)
    {
        switch (curLevel.theme)
        {
            case GlobalVars.DungeonTheme.DEFAULT:
                mainMusicSource.clip = musicList[1];
                break;
            case GlobalVars.DungeonTheme.CAVE:
                mainMusicSource.clip = musicList[1];
                break;
            case GlobalVars.DungeonTheme.CASTLE:
                mainMusicSource.clip = musicList[2];
                break;
            case GlobalVars.DungeonTheme.SKY:
                if (curLevel.sceneType == GlobalVars.SceneType.BOSS)
                {
                    mainMusicSource.clip = musicList[4];
                }
                else
                {
                    mainMusicSource.clip = musicList[3];
                }
                break;
        }

        if (curLevel.theme != prevTheme) { mainMusicSource.Play(); }
        prevTheme = curLevel.theme;
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

    public void SetPrevTheme(GlobalVars.DungeonTheme theme) { prevTheme = theme; }
}
