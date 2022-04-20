using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderBar : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    private float targetValue = -1.0f;
    [SerializeField, Tooltip("Speed that the slider changes, set to 0 to disable lerping")]
    private float lerpSpeed = 3.0f;

    private void Start()
    {
        // Set the target value to be the current value, unless a different instruction is given
        if(targetValue < 0.0f)
            targetValue = fill.fillAmount;
    }

    private void Update()
    {
        // Don't update when the target is reached
        if (targetValue == fill.fillAmount)
            return;

        // If the target is almmost reached, set the value
        if (MathUtils.AlmostZero(targetValue - fill.fillAmount, 3))
        {
            fill.fillAmount = targetValue;
            return;
        }

        // Lerp to the target value
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, targetValue, lerpSpeed * Time.deltaTime);
    }

    // Updates the fill percent of the health bar
    public void UpdateValue(float percent)
    {
        targetValue = percent;

        // Don't lerp on negative speeds
        if (lerpSpeed <= 0.0f)
            fill.fillAmount = percent;
    }
}
