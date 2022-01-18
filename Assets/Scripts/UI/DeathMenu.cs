using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuHolder;
    [SerializeField]
    private GameObject firstSelectedObject;

    // Enables the death menu
    public void EnableMenu(bool shouldEnable)
    {
        menuHolder.SetActive(shouldEnable);

        // Selects the first button
        //if (shouldEnable)
        //    UISelectionManager.Instance.SetDefaultSelection(firstSelectedObject);
    }
}
