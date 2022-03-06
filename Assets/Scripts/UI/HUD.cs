using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject hudObj;
    [SerializeField]
    private UIHealthBar healthBar;
    [SerializeField]
    private UICooldown dashCooldown;
    [SerializeField]
    private UIMinimap minimap;
    [SerializeField]
    private UIDialogueBox dialogueBox;
    [SerializeField]
    private bool isActive;

    // Enables the hud. Passing false allows the same function to disable the hud
    public void EnableHUD(bool shouldEnable = true)
    {
        hudObj.SetActive(shouldEnable);
        isActive = shouldEnable;
    }

    // Refreshes the HUD, getting information from relevant managers
    public void RefreshHUD()
    {
        // Refreshes which part of the HUD should be displayed based on whether it is singleplayer
        if(isActive)
            EnableHUD();

        // Update parts of the hud
    }

    /* ============================================================ Child component function wrappers ==================================== */
    public void EnableDialogue(bool shouldEnable = true) { dialogueBox.EnableDialogue(shouldEnable); }
    public void ContinueDialogue() { dialogueBox.ContinueDialogue(); }
    public void UpdateHealth(float percent) { healthBar.UpdateHealth(percent); }
    public void UpdateDashCooldown(float percent) { dashCooldown.UpdateCooldown(percent); }
}
