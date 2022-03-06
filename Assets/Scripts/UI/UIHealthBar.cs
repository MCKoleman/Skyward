using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image fill;

    // Updates the fill percent of the health bar
    public void UpdateHealth(float percent)
    {
        fill.fillAmount = percent;
    }
}
