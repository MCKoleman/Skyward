using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAbilityHandler : MonoBehaviour
{
    [Header("Abilities")]
    [SerializeField]
    private List<UICooldown> abilityList;
    [SerializeField]
    private UICooldown dashCooldown;
    [SerializeField]
    private UICooldown shieldCooldown;
    [SerializeField]
    private UICooldown ability1Cooldown;
    [SerializeField]
    private UICooldown ability2Cooldown;
    [SerializeField]
    private UICooldown ability3Cooldown;
    [SerializeField]
    private UICooldown spellCooldown;

    [Header("Slots")]
    [SerializeField]
    private Transform activeSlot;
    [SerializeField]
    private List<Transform> slots;
    private int highestFreeSlot = 0;

    // Selects the given ability type as the active type. If it is already selected, switch to default ability
    public GlobalVars.AbilityType SelectAbility(GlobalVars.AbilityType type, GlobalVars.AbilityType activeType)
    {
        // Don't do anything if trying to reselect magic missile
        if (activeType == GlobalVars.AbilityType.MAGIC_MISSILE && (activeType == type))
            return activeType;

        // Unselect all abilities
        for (int i = 0; i < abilityList.Count; i++)
            ReparentTransform(abilityList[i].transform, this.transform);
        highestFreeSlot = 0;

        // If selecting the same ability again, switch to magic missile
        activeType = ((activeType == type) ? GlobalVars.AbilityType.MAGIC_MISSILE : type);
        UICooldown temp = GetAbilityElement(activeType);

        // Select abilities
        ReparentTransform(temp.transform, activeSlot);
        for(int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i] != temp && highestFreeSlot < slots.Count)
            {
                ReparentTransform(abilityList[i].transform, slots[highestFreeSlot]);
                highestFreeSlot++;
            }
        }

        return activeType;
    }

    // Reparents the given transform to be a child of the given parent
    private void ReparentTransform(Transform target, Transform parent)
    {
        target.SetParent(parent, false);
    }

    // Returns the UICooldown with the given type
    public UICooldown GetAbilityElement(GlobalVars.AbilityType type)
    {
        // Return nothing for default
        if (type == GlobalVars.AbilityType.DEFAULT)
            return null;
        // Return n-1th ability for valid abilities
        else
            return abilityList[Mathf.Clamp((byte)type - 1, 0, abilityList.Count)];
    }

    // Cooldown handles
    public void UpdateDashCooldown(float percent) { dashCooldown.UpdateCooldown(percent); }
    public void UpdateShieldCooldown(float percent) { shieldCooldown.UpdateCooldown(percent); }
    public void UpdateSpellCooldown(float percent) { spellCooldown.UpdateCooldown(percent); }

    public void UpdateAbility0Cooldown(float percent)
    {
        GetAbilityElement(GlobalVars.AbilityType.MAGIC_MISSILE).UpdateCooldown(percent);
    }

    public void UpdateAbility1Cooldown(float percent) 
    {
        ability1Cooldown.UpdateCooldown(percent);
        GetAbilityElement(GlobalVars.AbilityType.METEOR).UpdateCooldown(percent);
    }

    public void UpdateAbility2Cooldown(float percent)
    {
        ability2Cooldown.UpdateCooldown(percent);
        GetAbilityElement(GlobalVars.AbilityType.ICE_WAVE).UpdateCooldown(percent);
    }

    public void UpdateAbility3Cooldown(float percent)
    {
        ability3Cooldown.UpdateCooldown(percent);
        GetAbilityElement(GlobalVars.AbilityType.LIGHTNING_BOLT).UpdateCooldown(percent);
    }
}
