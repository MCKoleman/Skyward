using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField, Range(1.0f, 30.0f)]
    protected float minRegenTime;
    [SerializeField]
    protected float curRegenTime;
    [SerializeField, Range(0.1f, 10.0f)]
    protected float healthRegenRate;
    [SerializeField]
    protected bool isHealthRegenEnabled = true;
    protected bool isHealthRegenActive;

    protected void Update()
    {
        // Don't do anything while health regen is active
        if (!isHealthRegenEnabled || isHealthRegenActive || CurHealth == maxHealth)
            return;

        // Start health regen when the min time has been reached
        if (curRegenTime >= minRegenTime)
            StartCoroutine(HandleHealthRegen());
        // Handle regen timer
        else
            curRegenTime += Time.deltaTime;
    }

    // Handles health regeneration
    protected IEnumerator HandleHealthRegen()
    {
        isHealthRegenActive = true;
        float regenTime = 1.0f / healthRegenRate;

        // Keep regenerating health until fully regenerated or interrupted
        while(isHealthRegenActive && CurHealth < maxHealth)
        {
            // Add 1 health every 1/healthRegenRate seconds (healthRegenRate health per second)
            Heal(1);
            yield return new WaitForSeconds(regenTime);
        }
        StopHealthRegen();
    }

    // Resets the health regeneration process
    protected void StopHealthRegen()
    {
        isHealthRegenActive = false;
        curRegenTime = 0.0f;
    }

    public override void HandleDeath()
    {
        // Respawn until lives are out
        PlayerManager.Instance.RemoveLife(1);
        if(PlayerManager.Instance.GetLives() >= 0)
        {
            if (healthSFX[2] != null)
                aSrc.PlayOneShot(healthSFX[2]);

            CheckpointManager.Instance.ResetToCheckpoint();
            HandleStart();
            HandleHealthChange();
        }
        // Show death menu on death
        else
        {
            if (healthSFX[1] != null)
                aSrc.PlayOneShot(healthSFX[1]);

            UIManager.Instance.ShowDeathMenu();
        }
    }

    public override void TakeDamage(int _amount)
    {
        StopHealthRegen();
        base.TakeDamage(_amount);
    }

    public override void HandleHealthChange()
    {
        base.HandleHealthChange();
        UIManager.Instance.UpdateHealth(CurHealth / (float)maxHealth);
    }

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
