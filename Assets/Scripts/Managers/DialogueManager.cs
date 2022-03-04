using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private DialogueList dialogueList;
    [SerializeField]
    private SpeakerLibrary speakerLibrary;
    [SerializeField]
    private int currentDialogue;

    // Initializer for the manager. Should only be called from GameManager
    public void Init()
    {
        currentDialogue = 0;
    }

    // Returns the next dialogue item
    public DialogueItem GetNextDialogue()
    {
        // Get the dialogue at the given index and increment the index afterward (i++)
        return dialogueList.GetDialogueAtIndex(currentDialogue++);
    }

    // Returns the dialogue at the given index
    public DialogueItem GetDialogueAtIndex(int index)
    {
        return dialogueList.GetDialogueAtIndex(index);
    }

    // Returns the given speaker item
    public SpeakerItem GetSpeaker(GlobalVars.SpeakerType type)
    {
        SpeakerItem speaker;
        speakerLibrary.GetSpeaker(type, out speaker);
        return speaker;
    }

    // Returns the dialogue index
    public int GetDialogueIndex() { return currentDialogue; }
}
