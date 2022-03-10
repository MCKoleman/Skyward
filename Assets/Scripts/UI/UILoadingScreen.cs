using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tooltipBox;
    [SerializeField]
    private TextMeshProUGUI progressBox;
    [SerializeField]
    private TooltipList tooltipList;
    [SerializeField]
    private GameObject screenHolder;
    [SerializeField, Range(0.5f, 20.0f), Tooltip("Duration of each tooltip")]
    private float tooltipLength = 3.0f;

    private void OnEnable()
    {
        StartCoroutine(ManageTooltips());
        screenHolder.SetActive(true);
    }

    private void OnDisable()
    {
        StopCoroutine(ManageTooltips());
        screenHolder.SetActive(false);
    }

    private IEnumerator ManageTooltips()
    {
        while(true)
        {
            SetRandomTooltip();
            yield return new WaitForSecondsRealtime(tooltipLength);
        }
    }

    // Sets the text in the progress box to the given text
    public void SetProgressText(string text)
    {
        progressBox.text = text;
    }

    // Sets the tooltip text to be a random text
    private void SetRandomTooltip()
    {
        string text = tooltipList.GetRandomTooltip();
        tooltipBox.text = text;
    }
}
