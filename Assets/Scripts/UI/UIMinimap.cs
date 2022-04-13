using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIMinimap : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI btnText;

    // Add null guarding to getting the value of controller
    private MinimapCameraController m_controller;
    private MinimapCameraController Controller {
        get
        {
            if (m_controller == null)
                m_controller = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<MinimapCameraController>();

            return m_controller;
        }
        set
        {
            m_controller = value;
        }
    }

    // Sets the width required for the camera to display the entire dungeon
    public void SetCameraWidth(float width)
    {
        Controller.SetCameraWidth(width);
    }

    // Sets the dungeon center to the given position
    public void SetDungeonCenter(Vector3 center)
    {
        Controller.SetDungeonCenter(center);
    }

    // Toggles the display mode of the minimap
    public void ToggleDisplayMode()
    {
        Controller.ToggleDisplayMode();
        btnText.text = Controller.GetIsPlayerView() ? "Dungeon View" : "Player View";
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Sets the display mode to map display
    public void SetDisplayModeFull()
    {
        Controller.SetDisplayMode(false);
        btnText.text = "Player View";
    }

    // Sets the display mode to player display
    public void SetDisplayModePlayer()
    {
        Controller.SetDisplayMode(true);
        btnText.text = "Dungeon View";
    }
}
