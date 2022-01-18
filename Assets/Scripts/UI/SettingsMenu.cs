using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    // Settings UI components
    [SerializeField]
    private OptionBoxUI qualityBox;
    [SerializeField]
    private OptionBoxUI resolutionBox;
    [SerializeField]
    private OptionBoxUI windowBox;
    [SerializeField]
    private Slider masterVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private TextMeshProUGUI masterVolumeText;
    [SerializeField]
    private TextMeshProUGUI musicVolumeText;
    [SerializeField]
    private TextMeshProUGUI sfxVolumeText;

    // Settings variables
    [SerializeField]
    private AudioMixer audioMixer;
    private Resolution[] resolutions;
    private Resolution curResolution;
    private FullScreenMode curFullscreenMode;
    private float[] decibelScale = new float[] { -80.0f, -40.0f, -30.0f, -25.0f, -20.0f, -15.0f, -10.0f, -8.0f, -5.0f, -2.0f, 0.0f};

    void Start()
    {
        // Init variables
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        // Find all possible resolutions
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // If the found resolution is the user's current resolution, store it
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Init option boxes
        resolutionBox.InitOptions(options, currentResolutionIndex);
        qualityBox.SetIndex(QualitySettings.GetQualityLevel());
        windowBox.SetIndex((int)Screen.fullScreenMode);

        // Init current volume to slider default
        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
    }

    // Sets the option of the given type to the given value
    public void SetOption(OptionBoxUI.OptionType option, int value)
    {
        switch(option)
        {
            case OptionBoxUI.OptionType.QUALITY:
                SetQuality(value);
                break;
            case OptionBoxUI.OptionType.RESOLUTION:
                SetResolution(value);
                break;
            case OptionBoxUI.OptionType.WINDOW:
                // Sets the window mode to the given value or default Windowed if value is 2 or more 
                SetWindow((int)value >= 2 ? 3 : value);
                break;
            default:
                break;
        }
    }

    // Sets the resolution of the game window
    public void SetResolution(int resolutionIndex)
    {
        curResolution = resolutions[resolutionIndex];
        Screen.SetResolution(curResolution.width, curResolution.height, curFullscreenMode);
    }

    // Sets the graphics quality of the game
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Sets the game game window to be fullscreen
    public void SetWindow(int windowIndex)
    {
        curFullscreenMode = (FullScreenMode)windowIndex;
        Screen.SetResolution(curResolution.width, curResolution.height, curFullscreenMode);
    }

    // Sets the volume of the game
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", decibelScale[Mathf.Clamp(Mathf.FloorToInt(volume), 0, decibelScale.Length - 1)]);
        masterVolumeText.text = Mathf.FloorToInt(volume).ToString();
    }

    // Sets the volume of the game
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", decibelScale[Mathf.Clamp(Mathf.FloorToInt(volume), 0, decibelScale.Length - 1)]);
        musicVolumeText.text = Mathf.FloorToInt(volume).ToString();
    }

    // Sets the volume of the game
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", decibelScale[Mathf.Clamp(Mathf.FloorToInt(volume), 0, decibelScale.Length - 1)]);
        sfxVolumeText.text = Mathf.FloorToInt(volume).ToString();
    }
}
