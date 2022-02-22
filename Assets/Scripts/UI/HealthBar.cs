using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image fill;

    // Sets the fill percentage of the health bar to the given percentage
    public void UpdateHealth(float percent)
    {
        fill.fillAmount = percent;
    }

}
