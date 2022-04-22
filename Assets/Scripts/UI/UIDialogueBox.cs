using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIDialogueBox : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject dialogueHolder;
    [SerializeField]
    private GameObject continueBtn;
    [SerializeField]
    private TextMeshProUGUI textBox;
    [SerializeField]
    private TextMeshProUGUI speakerBox;
    [SerializeField]
    private Image bgImg;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 100.0f), Tooltip("How many characters should be revealed per second")]
    private float dialogueRevealSpeed = 10.0f;

    [Header("Runtime information")]
    [SerializeField]
    private Image speakerImage;
    [SerializeField]
    private DialogueItem currentDialogue;
    [SerializeField]
    private DialogueStruct currentDialogueStruct;
    [SerializeField]
    private SpeakerItem currentSpeaker;
    [SerializeField]
    private int currentDialogueIndex = 0;

    private void Start()
    {
        EnableDialogue(false);
    }

    // Enables (by default) or disables the dialogue box
    public void EnableDialogue(bool shouldEnable = true)
    {
        UIManager.Instance.ShowHUDAbilities(!shouldEnable);
        dialogueHolder.SetActive(shouldEnable);
    }

    // Continues the dialogue, 
    public void ContinueDialogue()
    {
        // Disable continue button
        continueBtn.SetActive(false);
        EnableDialogue(true);

        // If this is not the last dialogue piece from the dialogue set, play the next one
        if(currentDialogueIndex < currentDialogue.GetDialogueCount() - 1)
        {
            currentDialogueIndex++;
            PlayNewDialogue();
            return;
        }

        // If this dialogue has finished, get new dialogue
        currentDialogue = DialogueManager.Instance.ContinueDialogue();
        currentDialogueIndex = 0;

        // If there is any dialogue left
        if(currentDialogue.GetDialogueCount() > 0)
        {
            PlayNewDialogue();
        }
        // If no new dialogue was given, disable the dialogue box
        else
        {
            EndDialogue();
        }
    }

    // Ends dialogue, resuming the game
    public void EndDialogue()
    {
        PrefabManager.Instance.ClearVisages();
        EnableDialogue(false);
        GameManager.Instance.SetTimeScale(1.0f);

        if (PrefabManager.Instance.IsBossAlive())
            PrefabManager.Instance.AwakeBoss();
    }

    // Plays a new dialogue, setting all necessary information in the process
    private void PlayNewDialogue()
    {
        currentDialogueStruct = currentDialogue.GetDialogueAtIndex(currentDialogueIndex);
        currentSpeaker = DialogueManager.Instance.GetSpeaker(currentDialogueStruct.speaker);
        speakerImage.sprite = currentSpeaker.sprite;
        textBox.text = "";
        speakerBox.text = currentSpeaker.name;

        // Set dialogue colors
        textBox.color = currentSpeaker.textColor;
        bgImg.color = currentSpeaker.bgColor;
        speakerBox.color = currentSpeaker.nameColor;

        // Play the next dialogue
        StartCoroutine(RevealText(currentDialogueStruct.text));
    }

    // Slowly reveals the current text item
    private IEnumerator RevealText(string text)
    {
        float timeDif = 1.0f / dialogueRevealSpeed;
        string curText = "";
        int charIndex = 0;

        // Slowly fill in the current text 
        while(curText != text)
        {
            // Add the target text to the text box one character at a time
            curText += text[charIndex];
            yield return new WaitForSecondsRealtime(timeDif * DialogueManager.Instance.GetDialogueSpeed());
            charIndex++;

            // Set the text component
            textBox.text = curText;
        }

        // Enable continue button
        continueBtn.SetActive(true);
        EventSystem.current.SetSelectedGameObject(continueBtn);
    }

    // Returns whether dialogue is active or not
    public bool IsDialogueActive()
    {
        return dialogueHolder.activeInHierarchy;
    }
}
