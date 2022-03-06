using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldown : MonoBehaviour
{
    [SerializeField]
    private GameObject cooldown;
    [SerializeField]
    private Image fill;

    // Updates the cooldowns fill percentage, enabling or disabling it based on the percentage
    public void UpdateCooldown(float percent)
    {
        cooldown.SetActive(percent > 0);
        fill.fillAmount = percent;
    }
}
