using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int baseMoveSpeed;
    [SerializeField]
    protected float movementSpeedMod = 1.0f;

    protected bool invincible = false;
    protected CharacterController controller;

    public int CurHealth { get; protected set; }
    public float CurMoveSpeed { get; protected set; }

    protected void Start()
    {
        HandleStart();
    }

    /// <summary>
    /// Handles construction for the character
    /// </summary>
    protected virtual void HandleStart()
    {
        controller = GetComponent<CharacterController>();
        CurMoveSpeed = baseMoveSpeed * movementSpeedMod;
        CurHealth = maxHealth;
        HandleHealthChange();
    }

    // Handles health changes (should be overriden to update UI etc)
    public virtual void HandleHealthChange()
    {

    }

    /// <summary>
    /// Heals the character for the given amount
    /// </summary>
    /// <param name="_amount">Amount to heal the character by</param>
    public virtual void Heal(int _amount)
    {
        CurHealth = Mathf.Clamp(CurHealth + _amount, 0, maxHealth);
        HandleHealthChange();
    }

    /// <summary>
    /// Takes damage for the character
    /// </summary>
    /// <param name="_amount">Amount of damage that the character takes</param>
    public virtual void TakeDamage(int _amount)
    {
        if (!invincible) {

            CurHealth = Mathf.Clamp(CurHealth - _amount, 0, maxHealth);
            HandleHealthChange();
            controller.HandleTakeDamage(IsDead());

            // Checks whether the character is dead
            if (IsDead())
                HandleDeath();
        }
    }

    /// <summary>
    /// Returns whether the character is a player or not
    /// </summary>
    /// <returns>Is character player</returns>
    public virtual bool IsPlayer()
    {
        // By default every character is not a player. This should be overloaded to be true in playerCharacter
        return false;
    }

    /// <summary>
    /// Returns whether the character is dead or not
    /// </summary>
    /// <returns>Is the character dead</returns>
    public bool IsDead()
    {
        return CurHealth <= 0;
    }

    /// <summary>
    /// Handles the death of the character
    /// </summary>
    public virtual void HandleDeath()
    {
        //Debug.Log($"{this.gameObject.name} died.");
        Destroy(this.gameObject);
    }

    public virtual void ToggleInvincibility(bool setTo)
    {
        invincible = setTo;
    }
}
