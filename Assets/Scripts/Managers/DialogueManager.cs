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
    [SerializeField]
    private int maxUnlockedDialogue;
    [SerializeField]
    private float curDialogueSpeedMultiplier = 1.0f;
    private const float SPED_UP_DIALOGUE_MULTIPLIER = 0.33f;

    // Initializer for the manager. Should only be called from GameManager
    public void Init()
    {
        currentDialogue = 0;
    }

    // Wrapper for delayed dialogue handling
    public void BeginDelayedAsyncDialogue()
    {
        StartCoroutine(DelayedAsyncDialogueHandle());
    }
    
    // Delays dialogue until __ is finished
    private IEnumerator DelayedAsyncDialogueHandle()
    {
        // Wait for game to start before showing dialogue
        if(!GameManager.Instance.IsGameActive)
            yield return new WaitUntil(() => GameManager.Instance.IsGameActive);

        BeginDialogue();
    }

    // Starts dialogue, setting the new max dialogue index to be 1 higher
    public void BeginDialogue()
    {
        BeginDialogue(maxUnlockedDialogue + 1);
    }

    // Starts dialogue, setting the new max dialogue index
    public void BeginDialogue(int newMaxIndex)
    {
        SetMaxUnlockedDialogue(newMaxIndex);
        GameManager.Instance.SetTimeScale(0.0f);
        UIManager.Instance.ContinueDialogue();
    }

    // Continues dialogue, either returning the highest unlocked dialogue or a null dialogue item
    public DialogueItem ContinueDialogue()
    {
        // Only give the next dialogue if it has been unlocked and exists
        if (currentDialogue < maxUnlockedDialogue && currentDialogue < dialogueList.Count)
            return GetNextDialogue();
        // Otherwise, return null dialogue
        else
            return new DialogueItem();
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

    // Sets the dialogue speed to fast or normal
    public void EnableFastDialogue(bool shouldEnable)
    {
        // Enables fast dialogue
        if (shouldEnable)
            curDialogueSpeedMultiplier = SPED_UP_DIALOGUE_MULTIPLIER;
        // Disables fast dialogue
        else
            curDialogueSpeedMultiplier = 1.0f;
    }

    // Returns the current dialogue speed
    public float GetDialogueSpeed() { return curDialogueSpeedMultiplier; }

    // Sets the new highest possible dialogue index
    public void SetMaxUnlockedDialogue(int _max) { maxUnlockedDialogue = _max; }

    // Returns the highest unlocked dialogue index
    public int GetMaxUnlockedDialogue() { return maxUnlockedDialogue; }

    // Returns the dialogue index
    public int GetDialogueIndex() { return currentDialogue; }
}
