using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    protected override void HandleStart()
    {
        base.HandleStart();
        Debug.Log("Started player character");
    }

    public override bool IsPlayer()
    {
        // A player character will always be player, every other character will not be
        return true;
    }
}
