using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIIntroDialogue : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject introHolder;
    [SerializeField]
    private GameObject continueBtn;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject mainMenuFirstBtn;
    [SerializeField]
    private TextMeshProUGUI textBox;
    [SerializeField]
    private Image bgImage;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 100.0f), Tooltip("How many characters should be revealed per second")]
    private float dialogueRevealSpeed = 70.0f;
    [SerializeField]
    private List<UIIntroStruct> introSlides;

    [Header("Runtime information")]
    [SerializeField]
    private UIIntroStruct currentSlide;
    [SerializeField]
    private int currentSlideIndex = 0;
    [SerializeField]
    private int currentDialogueIndex = 0;
    [SerializeField]
    private float currentDialogueSpeed = 1.0f;
    private const float FAST_DIALOGUE_SPEED = 0.33f;

    // Structs
    [System.Serializable]
    private struct UIIntroStruct {
        public Sprite image;
        public List<string> textList;
    }

    // Disable the main menu and start dialogue as soon as possible
    private void Awake()
    {
        introHolder.SetActive(true);
        mainMenu.SetActive(false);

        // Get first slide
        if(introSlides.Count > 0)
            currentSlide = introSlides[0];

        // Start intro
        ContinueDialogue();
    }

    // TEMP: REPLACE THIS WITH NEW INPUT SYSTEM ASAP
    // ---------------------------------------------
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            SetFastDialogue(true);
        else if(Input.GetKeyUp(KeyCode.Space))
            SetFastDialogue(false);
    }
    // ---------------------------------------------
    // TEMP: REPLACE THIS WITH NEW INPUT SYSTEM ASAP


    // Continues the dialogue until the intro is over
    public void ContinueDialogue()
    {
        // Disable the continue button
        continueBtn.SetActive(false);

        // Continue dialogue as long as there are options
        if (currentSlideIndex < introSlides.Count)
        {
            // If the dialogue index has gone past the max, select the next slide
            if(currentDialogueIndex >= introSlides[currentSlideIndex].textList.Count)
            {
                currentDialogueIndex = 0;
                currentSlideIndex++;

                // If the next slide index is still valid, get the next slide
                if (currentSlideIndex < introSlides.Count && introSlides[currentSlideIndex].textList != null)
                    currentSlide = introSlides[currentSlideIndex];
                // If the slide is invalid, break
                else
                    currentSlide = new UIIntroStruct();
            }

            // If the current slide is valid, start the next dialogue
            if(currentSlide.textList != null)
            {
                bgImage.sprite = currentSlide.image;
                textBox.text = "";
                StartCoroutine(RevealText(currentSlide.textList[currentDialogueIndex]));
                currentDialogueIndex++;
                return;
            }
        }

        // Enable main menu and disable intro
        mainMenu.SetActive(true);
        introHolder.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstBtn);
    }

    // Enables or disables fast dialogue
    public void SetFastDialogue(bool shouldEnable)
    {
        currentDialogueSpeed = shouldEnable ? FAST_DIALOGUE_SPEED : 1.0f;
    }

    // Slowly reveals the current text item
    private IEnumerator RevealText(string text)
    {
        float timeDif = 1.0f / dialogueRevealSpeed;
        string curText = "";
        int charIndex = 0;

        // Slowly fill in the current text 
        while (curText != text)
        {
            // Add the target text to the text box one character at a time
            curText += text[charIndex];
            yield return new WaitForSecondsRealtime(timeDif * currentDialogueSpeed);
            charIndex++;

            // Set the text component
            textBox.text = curText;
        }

        // Enable continue button
        continueBtn.SetActive(true);
        EventSystem.current.SetSelectedGameObject(continueBtn);
    }
}
