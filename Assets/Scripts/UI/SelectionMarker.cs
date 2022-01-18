using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionMarker : MonoBehaviour
{
    [SerializeField]
    private GameObject markerL;
    [SerializeField]
    private GameObject markerR;
    [SerializeField]
    private float selectionDistance;

    private GameObject curSelectedObj;

    private void Update()
    {
        // TODO: Move this from update, terrible performance

        // Only reselect if the currently selected object is different from last frame
        if (curSelectedObj != EventSystem.current.currentSelectedGameObject)
            SetSelection(EventSystem.current.currentSelectedGameObject);
    }

    // Sets the selection to the given game object
    private void SetSelection(GameObject obj)
    {
        // Only set selection if the given object is valid
        if(obj != null)
        {
            this.transform.position = obj.transform.position;
            curSelectedObj = obj;
        }
        SetMarkers();
    }

    // Moves the markers to be the 
    private void SetMarkers()
    {
        float tempSize = curSelectedObj.GetComponent<RectTransform>().rect.width / 2 + selectionDistance;
        markerL.transform.localPosition = new Vector3(-tempSize, 0.0f, 0.0f);
        markerR.transform.localPosition = new Vector3(tempSize, 0.0f, 0.0f);
    }
}
