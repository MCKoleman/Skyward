using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIOptionBox : MonoBehaviour
{
    public enum OptionType { QUALITY, RESOLUTION, WINDOW }

    [SerializeField]
    private OptionType optionType;
    [SerializeField]
    private SettingsMenu settingsMenu;
    [SerializeField]
    private TextMeshProUGUI valueText;
    [SerializeField]
    private Button leftBtn;
    [SerializeField]
    private Button rightBtn;
    [SerializeField]
    private Image leftBtnImg;
    [SerializeField]
    private Image rightBtnImg;
    [SerializeField]
    private bool leftBtnEnabled;
    [SerializeField]
    private bool rightBtnEnabled;
    [SerializeField]
    private Color enabledColor;
    [SerializeField]
    private Color disabledColor;

    // Info
    [SerializeField]
    private List<string> options;
    [SerializeField]
    private int curIndex;

    // Initializes the button to display correctly
    public void InitOptions(List<string> _options, int startIndex)
    {
        curIndex = startIndex;
        options = _options;
        UpdateButtonStates();
    }

    // Sets the option boxes index
    public void SetIndex(int index)
    {
        curIndex = index;
        UpdateButtonStates();
    }

    // Updates the enabled status of the buttons
    public void UpdateButtonStates()
    {
        // Switches interaction to the right button
        if(curIndex <= 0 && leftBtnEnabled)
        {
            EventSystem.current.SetSelectedGameObject(rightBtn.gameObject);
        }
        // Switches interaction to the left button
        if(curIndex >= options.Count - 1 && rightBtnEnabled)
        {
            EventSystem.current.SetSelectedGameObject(leftBtn.gameObject);
        }
        
        // Set button interaction states
        leftBtnEnabled = curIndex > 0;
        rightBtnEnabled = curIndex < options.Count - 1;
        leftBtnImg.color = leftBtnEnabled ? enabledColor : disabledColor;
        rightBtnImg.color = rightBtnEnabled ? enabledColor : disabledColor;

        // Update display and settings
        UpdateText();
        settingsMenu.SetOption(optionType, curIndex);
    }

    // Updates the displayed text to the stored text
    public void UpdateText()
    {
        valueText.text = options[Mathf.Clamp(curIndex, 0, options.Count - 1)];
    }

    // Cycles the selection state to the left
    public void CycleLeft()
    {
        if (curIndex > 0)
            curIndex--;
        UpdateButtonStates();
    }

    // Cycles the selection state to the right
    public void CycleRight()
    {
        if (curIndex < options.Count - 1)
            curIndex++;
        UpdateButtonStates();
    }
}
