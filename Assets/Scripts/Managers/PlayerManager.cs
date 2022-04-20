using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private int curXP;
    [SerializeField]
    private int xpToLevelUp;
    [SerializeField]
    private int curLives;

    // Initializes the PlayerManager. Should only be called from GameManager
    public void Init()
    {
        curXP = 0;
        curLives = 3;
        UpdateUIDisplay();
    }

    // Handles the final death
    public void HandleFinalDeath()
    {

    }

    // Updates the UI display
    public void UpdateUIDisplay()
    {
        UIManager.Instance.UpdateXpDisplay(GetXpPercent());
        UIManager.Instance.UpdateLifeDisplay(GetLives());
    }

    // Adds xp to the player
    public void AddXp(int xpToAdd)
    {
        // Don't use XP in easy mode
        if (GameManager.Instance.GetIsEasyMode())
            return;

        // Add xp
        curXP += xpToAdd;

        // If enough XP is gained, add a life
        if(curXP >= xpToLevelUp)
        {
            curXP -= xpToLevelUp;
            curLives++;
            UIManager.Instance.UpdateLifeDisplay(GetLives());
        }
        UIManager.Instance.UpdateXpDisplay(GetXpPercent());
    }
    
    // Adds the given amount of lives (default 1)
    public void AddLife(int _lives = 1)
    {
        // Don't use XP in easy mode
        if (GameManager.Instance.GetIsEasyMode())
            return;

        curLives += _lives;
        UIManager.Instance.UpdateLifeDisplay(GetLives());
    }

    // Removes lives from the player (default 1)
    public void RemoveLife(int _lives = 1)
    {
        // Don't use XP in easy mode
        if (GameManager.Instance.GetIsEasyMode())
            return;

        curLives -= _lives;
        UIManager.Instance.UpdateLifeDisplay(GetLives());

        // Kill the player on negative lives
        if (curLives < 0)
            HandleFinalDeath();
    }

    // Returns the xp perecentage
    public float GetXpPercent()
    {
        if (GameManager.Instance.GetIsEasyMode())
            return 1.0f;
        else
            return (float)curXP / (float)xpToLevelUp;
    }

    // Returns the current lives
    public int GetLives()
    {
        if (GameManager.Instance.GetIsEasyMode())
            return int.MaxValue;
        else
            return curLives;
    }

    // Returns whether the player is on their final life
    public bool IsFinalLife() { return curLives == 0; }
}
