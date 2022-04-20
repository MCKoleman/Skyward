using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuHolder;
    [SerializeField]
    private GameObject firstSelectedObject;

    // Enables or disables the pause menu
    public void EnableMenu(bool shouldEnable)
    {
        menuHolder.SetActive(shouldEnable);
    }

    // Pauses the game
    public void PauseGame()
    {
        // If the game cannot be paused, do not act on this call
        if (!GameManager.Instance.CanPause())
            return;

        EnableMenu(true);
        GameManager.Instance.SetGameState(GameManager.GameState.PAUSED, true);
        //Cursor.visible = true;

        // Redundant call to make sure button is selected
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        //UISelectionManager.Instance.SetDefaultSelection(firstSelectedObject);
    }

    // Unpauses the game
    public void UnpauseGame()
    {
        GameManager.Instance.RevertToPreviousGameState();
        //Cursor.visible = false;
        EnableMenu(false);
    }
}
