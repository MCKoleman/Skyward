using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField]
    private UISliderBar healthBar;
    [SerializeField]
    private GameObject minimapIcon;
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

        if (CurHealth <= 0 && healthSFX[1] != null)
            aSrc.PlayOneShot(healthSFX[1]);
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
        Destroy(minimapIcon);

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
