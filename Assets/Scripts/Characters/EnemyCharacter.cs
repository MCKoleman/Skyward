using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private UISliderBar healthBar;
    [SerializeField]
    protected float invincTime = 0.25f;
    [SerializeField]
    protected bool destroyOnDeath = true;
    [SerializeField]
    protected int xp;

    public override void HandleHealthChange()
    {
        base.HandleHealthChange();
        healthBar?.UpdateValue(CurHealth / (float)maxHealth);
    }

    IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(invincTime);
        invincible = false;
    }

    public override void HandleDeath()
    {
        // Handle xp first
        PlayerManager.Instance.AddXp(xp);

        if (destroyOnDeath)
        {
            Destroy(this.gameObject);
        }

        healthBar.gameObject.SetActive(false);
        invincible = true;
    }

    public override void TakeDamage(int _amount)
    {
        base.TakeDamage(_amount);

        //Make sure player damage only hurts once
        if (!invincible)
        {
            invincible = true;
            StartCoroutine(Invincibility());
        }
    }
}
