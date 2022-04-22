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
    private Color targetColor;
    [SerializeField]
    private bool reversed = true;
    private const float LERP_SPEED = 3.0f;

    private void Start()
    {
        UpdateCooldown(0.0f);
    }

    private void Update()
    {
        // Don't update when the target is reached
        if (targetColor == keybind.color)
            return;

        // If the target is almmost reached, set the value
        if (targetColor.CompareRGB(keybind.color))
        {
            keybind.color = targetColor;
            return;
        }

        // Lerp to the target value
        keybind.color = Color.Lerp(keybind.color, targetColor, LERP_SPEED * Time.deltaTime);
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
        // Only lerp color one direction
        if (percent > 0)
            keybind.color = cooldownColor;
        else
            targetColor = offCooldownColor;

        onCooldown.SetActive(percent > 0);
        offCooldown.SetActive(percent > 0);
        ready.SetActive(percent <= 0);

        // Allow reversing of the fill
        cooldownFill.fillAmount = (reversed ? (1 - percent) : percent);
    }
}
