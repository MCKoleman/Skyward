using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("Prefabs")]
    // Player prefabs
    public GameObject[] enemyPrefabList;

    [Header("Holders")]
    // Object holders
    public Transform projectileHolder;
    public Transform enemyHolder;
    public Transform powerupHolder;
    public Transform levelHolder;
    public Transform bossHolder;

    // Initializes the manager. Should only be called from GameManager
    public void Init()
    {
        //ResetLevel();
    }

    // Resets all gameObjects
    public void ResetLevel()
    {
        for(int i = projectileHolder.childCount - 1; i >= 0; i--)
            Destroy(projectileHolder.GetChild(i).gameObject);

        for (int i = enemyHolder.childCount - 1; i >= 0; i--)
            Destroy(enemyHolder.GetChild(i).gameObject);

        for (int i = powerupHolder.childCount - 1; i >= 0; i--)
            Destroy(powerupHolder.GetChild(i).gameObject);

        for (int i = levelHolder.childCount - 1; i >= 0; i--)
            Destroy(levelHolder.GetChild(i).gameObject);

        for (int i = bossHolder.childCount - 1; i >= 0; i--)
            Destroy(bossHolder.GetChild(i).gameObject);
    }

    // Returns the number of enemies 
    public int GetEnemyCount() { return enemyHolder.childCount; }

    // Returns whether any bosses are alive
    public bool IsBossAlive() { return bossHolder != null && bossHolder.childCount > 0; }
}
