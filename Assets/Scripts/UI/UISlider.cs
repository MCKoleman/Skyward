using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI labelText;

    // Sets the value of the slider text
    public void SetLabel(float value)
    {
        labelText.text = Mathf.FloorToInt(value).ToString();
    }

    // Returns the slider's value
    public float GetValue() { return slider.value; }
}
