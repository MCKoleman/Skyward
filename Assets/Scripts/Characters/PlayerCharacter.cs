using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public int CurrentHealth;

    protected override void HandleStart()
    {
        base.HandleStart();
        Debug.Log("Started player character");
        CurrentHealth = CurHealth;
    }

    public override bool IsPlayer()
    {
        // A player character will always be player, every other character will not be
        return true;
    }
}
