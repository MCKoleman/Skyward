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
        UIManager.Instance.UpdateXpDisplay(GetXpPercent());
        UIManager.Instance.UpdateLifeDisplay(Mathf.Max(curLives, 0));
    }

    // Handles the final death
    public void HandleFinalDeath()
    {

    }

    // Adds xp to the player
    public void AddXp(int xpToAdd)
    {
        // Add xp
        curXP += xpToAdd;

        // If enough XP is gained, add a life
        if(curXP >= xpToLevelUp)
        {
            curXP -= xpToLevelUp;
            curLives++;
            UIManager.Instance.UpdateLifeDisplay(curLives);
        }
        UIManager.Instance.UpdateXpDisplay(GetXpPercent());
    }
    
    // Adds the given amount of lives (default 1)
    public void AddLife(int _lives = 1)
    {
        curLives += _lives;
        UIManager.Instance.UpdateLifeDisplay(curLives);
    }

    // Removes lives from the player (default 1)
    public void RemoveLife(int _lives = 1)
    {
        curLives -= _lives;
        UIManager.Instance.UpdateLifeDisplay(Mathf.Max(curLives, 0));

        // Kill the player on negative lives
        if (curLives < 0)
            HandleFinalDeath();
    }

    // Returns the xp perecentage
    public float GetXpPercent() { return (float)curXP / (float)xpToLevelUp; }
    // Returns the current lives
    public int GetLives() { return curLives; }
    // Returns whether the player is on their final life
    public bool IsFinalLife() { return curLives == 0; }
}
