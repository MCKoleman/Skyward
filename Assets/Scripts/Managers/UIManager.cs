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

    // Initializes the UI
    public void Init()
    {

    }

    // Initializes the hud
    public void InitHUD()
    {
        ShowPauseMenu(false);
        ShowDeathMenu(false);
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
        deathMenu.EnableMenu(shouldEnable);
    }

    // Displays the pause menu
    public void ShowPauseMenu(bool shouldEnable = true)
    {
        pauseMenu.EnableMenu(shouldEnable);
    }

    // Enables or disables the loading screen
    public void EnableLoadingScreen(bool shouldEnable = true)
    {
        loadingScreen.gameObject.SetActive(shouldEnable);
    }

    // Toggles the game's pause state
    public void PauseGameToggle()
    {
        if (Time.timeScale != 0.0f)
            PauseGame();
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
    public void UpdateHealth(float percent) { hud.UpdateHealth(percent); }
    public void UpdateDashCooldown(float percent) { hud.UpdateDashCooldown(percent); }
    public void ContinueDialogue() { hud.ContinueDialogue(); }
    public void SetLoadingProgressText(string text) { loadingScreen.SetProgressText(text); }
}
