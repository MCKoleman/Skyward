using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("Prefabs")]
    // Player prefabs
    public GameObject[] enemyPrefabList;

    [Header("Room prefabs")]
    public GameObject exitPrefab;
    public GameObject entrancePrefab;
    public GameObject baseRoomPrefab;
    public GameObject bossRoomPrefab;

    [Header("Wall prefabs")]
    [Tooltip("DEFAULT = 0, CAVE = 1, SKY = 2, CASTLE = 3")]
    public GameObject[] topWalls;
    public GameObject[] sideWalls;
    public GameObject[] topDoorWalls;
    public GameObject[] sideDoorWalls;
    public GameObject[] floors;

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

    // Returns a wall object of the given wall type and given theme
    public GameObject GetWallObject(GlobalVars.WallType wallType, GlobalVars.DungeonTheme themeType)
    {
        switch(wallType)
        {
            case GlobalVars.WallType.TOP:
                return topWalls[Mathf.Clamp((int)themeType, 0, topWalls.Length)];
            case GlobalVars.WallType.SIDE:
                return sideWalls[Mathf.Clamp((int)themeType, 0, sideWalls.Length)];
            case GlobalVars.WallType.TOP_DOOR:
                return topDoorWalls[Mathf.Clamp((int)themeType, 0, topDoorWalls.Length)];
            case GlobalVars.WallType.SIDE_DOOR:
                return sideDoorWalls[Mathf.Clamp((int)themeType, 0, sideDoorWalls.Length)];
            case GlobalVars.WallType.FLOOR:
                return floors[Mathf.Clamp((int)themeType, 0, floors.Length)];
            case GlobalVars.WallType.DEFAULT:
            default:
                return null;
        }
    }

    // Returns the number of enemies 
    public int GetEnemyCount() { return enemyHolder.childCount; }

    // Returns whether any bosses are alive
    public bool IsBossAlive() { return bossHolder != null && bossHolder.childCount > 0; }
}
