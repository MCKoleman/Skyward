using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBtnAudio : MonoBehaviour
{
    // Plays a random click sound effect
    public void PlayClick()
    {
        AudioManager.Instance.PlayUISubmit();
    }

    // Plays a random select sound effect
    public void PlaySelect()
    {
        AudioManager.Instance.PlayUISelect();
    }
}
