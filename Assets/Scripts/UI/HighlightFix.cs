using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class HighlightFix : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    private Button button;

    private void Start()
    {
        // Check for buttons
        button = this.GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Don't reselect if already selected, and don't select if this object is a non-interactable button
        if (!EventSystem.current.alreadySelecting && (button == null || button.interactable))
            EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.GetComponent<Selectable>().OnPointerExit(null);
    }
}
