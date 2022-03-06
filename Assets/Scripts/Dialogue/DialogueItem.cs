using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueItem
{
    [SerializeField]
    private List<DialogueStruct> dialogue;

    // Returns the dialogue at the given index
    public DialogueStruct GetDialogueAtIndex(int index)
    {
        return dialogue[index];
    }

    // Returns the speaker at the given index
    public GlobalVars.SpeakerType GetSpeakerAtIndex(int index)
    {
        return dialogue[index].speaker;
    }

    // Returns the text at the given index
    public string GetTextAtIndex(int index)
    {
        return dialogue[index].text;
    }

    // Returns the number of dialogue elements in this struct
    public int GetDialogueCount()
    {
        if(dialogue != null)
            return dialogue.Count;
        else
            return -1;
    }
}

[System.Serializable]
public struct DialogueStruct
{
    public GlobalVars.SpeakerType speaker;
    public string text;
}