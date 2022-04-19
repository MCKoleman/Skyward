using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool reversed = true;

    private void Start()
    {
        UpdateCooldown(0.0f);
    }

    // Updates the cooldowns fill percentage, enabling or disabling it based on the percentage
    public void UpdateCooldown(float percent)
    {
        onCooldown.SetActive(percent > 0);
        offCooldown.SetActive(percent > 0);
        ready.SetActive(percent <= 0);

        // Allow reversing of the fill
        cooldownFill.fillAmount = (reversed ? (1 - percent) : percent);
    }
}
