using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    // Major UI components
    [SerializeField]
    private HUD hud;
    [SerializeField]
    private PauseMenu pauseMenu;
    [SerializeField]
    private DeathMenu deathMenu;
    [SerializeField]
    private UILoadingScreen loadingScreen;
    [SerializeField]
    private EndScreen endScreen;

    private bool playedIntro = false;

    // Initializes the UI
    public void Init()
    {

    }

    // Initializes the hud
    public void InitHUD()
    {
        ShowPauseMenu(false);
        ShowDeathMenu(false);
        ShowEndScreen(false);
        ShowHUD(true);
        //Cursor.visible = (SceneManager.GetActiveScene().buildIndex == 0);
    }

    // Displays the HUD
    public void ShowHUD(bool shouldEnable = true)
    {
        hud.EnableHUD(shouldEnable);
    }

    // Displays the death menu
    public void ShowDeathMenu(bool shouldEnable = true)
    {
        hud.EnableHUD(!shouldEnable);
        deathMenu.EnableMenu(shouldEnable);
    }

    // Displays the pause menu
    public void ShowPauseMenu(bool shouldEnable = true)
    {
        pauseMenu.EnableMenu(shouldEnable);
    }

    // Displays the end screen
    public void ShowEndScreen(bool shouldEnable = true)
    {
        endScreen.EnableScreen(shouldEnable);
    }


    // Enables or disables the loading screen
    public void EnableLoadingScreen(bool shouldEnable = true)
    {
        loadingScreen.gameObject.SetActive(shouldEnable);
    }

    // Toggles the game's pause state
    public void PauseGameToggle()
    {
        // Don't allow pausing during dialogue
        if (hud.IsDialogueActive())
            return;

        // Pause an unpaused game
        if (Time.timeScale != 0.0f)
            PauseGame();
        // Unpause a paused game
        else
            UnpauseGame();
    }

    // Pauses the game. Should only happen if called from HUD.
    public void PauseGame()
    {
        pauseMenu.PauseGame();
    }

    // Unpauses the game. Should only happen if called from HUD.
    public void UnpauseGame()
    {
        pauseMenu.UnpauseGame();
    }

    // Handles the player leaving the scene or game
    public void HandleExit()
    {
        GameManager.Instance.SetTimeScale(1.0f);
        GameManager.Instance.EndGame();
    }

    // Returns whether the game is paused or not
    public bool IsPaused() { return Time.timeScale == 0.0f; }

    // Wrapper function for refreshing the HUD, callable from anywhere
    public void RefreshHUD() { hud.RefreshHUD(); }

    /* ============================================================ Child component function wrappers ==================================== */
    //public void UpdateLifeDisplay() { hud.UpdateLifeDisplay(); }
    public bool IsDialogueActive() { return hud.IsDialogueActive(); }
    public void UpdateHealth(float percent) { hud.UpdateHealth(percent); }
    public void ShowHUDAbilities(bool shouldEnable = true) { hud.ShowAbilities(shouldEnable); }
    public void ContinueDialogue() { hud.ContinueDialogue(); }
    public void EndDialogue() { hud.EndDialogue(); }
    public void SetLoadingProgressText(string text) { loadingScreen.SetProgressText(text); }
    public void SetMinimapCameraWidth(float width) { hud.SetMinimapCameraWidth(width); }
    public void SetMinimapDungeonCenter(Vector3 center) { hud.SetMinimapDungeonCenter(center); }
    public void SetLevelNum(int num) { hud.SetLevelNum(num); }
    public void UpdateXpDisplay(float percent) { hud.UpdateXpDisplay(percent); }
    public void UpdateLifeDisplay(int lives) { hud.UpdateLifeDisplay(lives); }
    public void SelectAbility(GlobalVars.AbilityType type) { hud.SelectAbility(type); }
    public void UpdateDashCooldown(float percent) { hud.UpdateDashCooldown(percent); }
    public void UpdateShieldCooldown(float percent) { hud.UpdateShieldCooldown(percent); }
    public void UpdateSpellCooldown(float percent) { hud.UpdateSpellCooldown(percent); }
    public void UpdateAbility0Cooldown(float percent) { hud.UpdateAbility0Cooldown(percent); }
    public void UpdateAbility1Cooldown(float percent) { hud.UpdateAbility1Cooldown(percent); }
    public void UpdateAbility2Cooldown(float percent) { hud.UpdateAbility2Cooldown(percent); }
    public void UpdateAbility3Cooldown(float percent) { hud.UpdateAbility3Cooldown(percent); }
    public void ResetAllCooldowns()
    {
        UpdateAbility0Cooldown(0.0f);
        UpdateAbility1Cooldown(0.0f);
        UpdateAbility2Cooldown(0.0f);
        UpdateAbility3Cooldown(0.0f);
        UpdateDashCooldown(0.0f);
        UpdateShieldCooldown(0.0f);
        UpdateSpellCooldown(0.0f);
    }
    public void IntroComplete() { playedIntro = true; }
    public bool IfIntroPlayed() { return playedIntro; }
}
