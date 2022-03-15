using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TooltipList", menuName = "ScriptableObjects/TooltipList", order = 1)]
[System.Serializable]
public class TooltipList : ScriptableObject
{
    public WeightedObjectList<string> tooltipList;

    // Returns a random tooltip
    public string GetRandomTooltip()
    {
        return tooltipList.GetRandomObject();
    }
}
