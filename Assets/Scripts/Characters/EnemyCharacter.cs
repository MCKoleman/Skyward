using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private UIHealthBar healthBar;
    [SerializeField]
    protected float invincTime = 0.25f;

    protected bool invincible = false;

    public override void HandleHealthChange()
    {
        base.HandleHealthChange();
        healthBar?.UpdateHealth(CurHealth / (float)maxHealth);
    }

    IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(invincTime);
        invincible = false;
    }

    public override void TakeDamage(int _amount)
    {
        if (!invincible)
        {
            base.TakeDamage(_amount);
            invincible = true;
            StartCoroutine(Invincibility());
        }
    }

    public void SetInvincible()
    {
        invincible = true;
    }
}
