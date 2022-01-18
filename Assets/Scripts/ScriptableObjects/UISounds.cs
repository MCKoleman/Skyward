using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UISounds", order = 1)]
[System.Serializable]
public class UISounds : ScriptableObject
{
    [SerializeField]
    private AudioClip[] selectClips;
    [SerializeField]
    private AudioClip[] clickClips;

    // Returns a random click clip
    public AudioClip GetClickClip()
    {
        return clickClips[Random.Range(0,clickClips.Length)];
    }

    // Returns a random select clip
    public AudioClip GetSelectClip()
    {
        return selectClips[Random.Range(0, selectClips.Length)];
    }
}
