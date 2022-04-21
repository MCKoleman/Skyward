using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICooldown : MonoBehaviour
{
    [SerializeField]
    private GameObject onCooldown;
    [SerializeField]
    private GameObject offCooldown;
    [SerializeField]
    private GameObject ready;
    [SerializeField]
    private Image cooldownFill;
    [SerializeField]
    private TextMeshProUGUI keybind;
    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private Color offCooldownColor;
    [SerializeField]
    private bool reversed = true;

    private void Start()
    {
        UpdateCooldown(0.0f);
    }

    // Sets the keybind of this cooldown
    public void SetKeybind(string _keybind)
    {
        keybind.text = _keybind;
    }

    // Handles resizing the cooldown component
    public void Resize(bool isSmall)
    {
        keybind.alignment = isSmall ? TextAlignmentOptions.BottomLeft : TextAlignmentOptions.Bottom;
    }

    // Updates the cooldowns fill percentage, enabling or disabling it based on the percentage
    public void UpdateCooldown(float percent)
    {
        keybind.color = (percent <= 0) ? offCooldownColor : cooldownColor;
        onCooldown.SetActive(percent > 0);
        offCooldown.SetActive(percent > 0);
        ready.SetActive(percent <= 0);

        // Allow reversing of the fill
        cooldownFill.fillAmount = (reversed ? (1 - percent) : percent);
    }
}
