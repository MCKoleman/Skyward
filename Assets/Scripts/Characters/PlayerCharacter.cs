using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
<<<<<<< HEAD
    public int CurrentHealth;
=======
    public override void HandleDeath()
    {
        CheckpointManager.Instance.ResetToCheckpoint();
        HandleStart();
        HandleHealthChange();
    }

    public override void HandleHealthChange()
    {
        base.HandleHealthChange();
        UIManager.Instance.UpdateHealth(CurHealth / (float)maxHealth);
    }
>>>>>>> 0711b6b30d1bfe326d1ebad86795c9cfa33220b2

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
