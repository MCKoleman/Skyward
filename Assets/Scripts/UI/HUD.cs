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
    private UISliderBar healthBar;
    [SerializeField]
    private UISliderBar xpBar;
    [SerializeField]
    private UIMinimap minimap;
    [SerializeField]
    private UIDialogueBox dialogueBox;
    [SerializeField]
    private UIAbilityHandler abilityHandler;
    [SerializeField]
    private TextMeshProUGUI levelNum;
    [SerializeField]
    private TextMeshProUGUI livesNum;
    [SerializeField]
    private bool isActive;

    // Enables the hud. Passing false allows the same function to disable the hud
    public void EnableHUD(bool shouldEnable = true)
    {
        hudObj.SetActive(shouldEnable);
        isActive = shouldEnable;
    }

    // Shows the ability displays
    public void ShowAbilities(bool shouldEnable = true)
    {
        abilityHandler.gameObject.SetActive(shouldEnable);
    }

    // Refreshes the HUD, getting information from relevant managers
    public void RefreshHUD()
    {
        // Refreshes which part of the HUD should be displayed based on whether it is singleplayer
        if(isActive)
            EnableHUD();

        // Update parts of the hud
    }

    // Sets the level number displayed
    public void SetLevelNum(int num)
    {
        levelNum.text = num.ToString();
    }

    /* ============================================================ Child component function wrappers ==================================== */
    public void EnableDialogue(bool shouldEnable = true) { dialogueBox.EnableDialogue(shouldEnable); }
    public void ContinueDialogue() { dialogueBox.ContinueDialogue(); }
    public void EndDialogue() { dialogueBox.EndDialogue(); }
    public void UpdateHealth(float percent) { healthBar.UpdateValue(percent); }
    public void SetMinimapCameraWidth(float width) { minimap.SetCameraWidth(width); }
    public void SetMinimapDungeonCenter(Vector3 center) { minimap.SetDungeonCenter(center); }
    public bool IsDialogueActive() { return dialogueBox.IsDialogueActive(); }
    public void UpdateXpDisplay(float percent) { xpBar.UpdateValue(percent); }
    public void UpdateLifeDisplay(int lives) { livesNum.text = GameManager.Instance.GetIsEasyMode() ? '\u221E'.ToString() : lives.ToString(); }
    public void SelectAbility(GlobalVars.AbilityType type) { abilityHandler.SelectAbility(type); }

    // Cooldown handles
    public void UpdateDashCooldown(float percent) { abilityHandler.UpdateDashCooldown(percent); }
    public void UpdateShieldCooldown(float percent) { abilityHandler.UpdateShieldCooldown(percent); }
    public void UpdateSpellCooldown(float percent) { abilityHandler.UpdateSpellCooldown(percent); }
    public void UpdateAbility0Cooldown(float percent) { abilityHandler.UpdateAbility0Cooldown(percent); }
    public void UpdateAbility1Cooldown(float percent) { abilityHandler.UpdateAbility1Cooldown(percent); }
    public void UpdateAbility2Cooldown(float percent) { abilityHandler.UpdateAbility2Cooldown(percent); }
    public void UpdateAbility3Cooldown(float percent) { abilityHandler.UpdateAbility3Cooldown(percent); }
}
