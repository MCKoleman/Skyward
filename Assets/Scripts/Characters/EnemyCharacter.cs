using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private UIHealthBar healthBar;

    public override void HandleHealthChange()
    {
        base.HandleHealthChange();
        healthBar?.UpdateHealth(CurHealth / (float)maxHealth);
    }
}
