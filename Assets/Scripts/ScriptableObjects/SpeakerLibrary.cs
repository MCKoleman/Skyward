using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerLibrary", menuName = "ScriptableObjects/SpeakerLibrary", order = 1)]
[System.Serializable]
public class SpeakerLibrary : ScriptableObject
{
    [SerializeField]
    private List<SpeakerItem> speakers;

    // Sets the speaker item to be the speaker searched for. Returns whether the speaker was found or not.
    public bool GetSpeaker(GlobalVars.SpeakerType type, out SpeakerItem speaker)
    {
        // Search for a speaker of the given type
        for(int i = 0; i < speakers.Count; i++)
        {
            // If the speaker is found, return it
            if (speakers[i].type == type)
            {
                speaker = speakers[i];
                return true;
            }
        }

        // Create default speaker if a speaker isn't found and throw an error
        Debug.LogError("SpeakerLibrary attempted to access a speaker that doesn't exist. Please make sure the speaker library has every speaker possible.");
        speaker = new SpeakerItem();
        return false;
    }
}
