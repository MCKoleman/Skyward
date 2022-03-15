using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFirstSelectFix : MonoBehaviour
{
    // If the event system does not have a selected gameObject, select this
    private void Start()
    {
        EventSystem current = EventSystem.current;
        Debug.Log($"Currently selected gameObject: [{current.currentSelectedGameObject}]");
        if (current != null && current.currentSelectedGameObject == null)
        {
            current.SetSelectedGameObject(this.gameObject);
        }
    }
}
