using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgressList", menuName = "ScriptableObjects/LevelProgressList", order = 1)]
[System.Serializable]
public class LevelProgressList : ScriptableObject
{
    [System.Serializable]
    public struct LevelProgress
    {
        public GlobalVars.SceneType sceneType;
        public GlobalVars.DungeonTheme theme;
    }

    public List<LevelProgress> progressList;

    // Returns the current level
    public LevelProgress GetCurrentLevel(int curLevel)
    {
        return progressList[Mathf.Clamp(GetNewCurLevel(curLevel), 0, progressList.Count)];
    }

    // Returns the desired sceneType of the next scene. If the scene progression is over, the next scene will be the main menu
    public GlobalVars.SceneType GetNextSceneType(int curLevel)
    {
        // Keep swapping levels forever in infinite
        int newLevelIndex = GetNewCurLevel(curLevel + 1);

        // If the next level still exists in the progression, return it
        if (newLevelIndex < progressList.Count)
            return progressList[newLevelIndex].sceneType;
        // Otherwise return menu
        else
            return GlobalVars.SceneType.MENU;
    }

    // Returns the converted level for use with infinite mode functions
    public int GetNewCurLevel(int curLevel)
    {
        if (GameManager.Instance.GetIsInfinite())
            return ((curLevel - 1) % (progressList.Count - 1)) + 1;

        return curLevel;
    }

    // Returns the current level index, skipping minibosses
    public int GetLevelMinusMiniBosses(int curLevel)
    {
        int levelToCheck = GetNewCurLevel(curLevel);

        int miniBosses = 0;
        for(int i = 0; i < Mathf.Min(levelToCheck+1, progressList.Count); i++)
        {
            if(progressList[i].sceneType == GlobalVars.SceneType.MINIBOSS)
                miniBosses++;
        }
        return curLevel - miniBosses;

        /*
        if (GameManager.Instance.GetIsInfinite())
            return curLevel - miniBosses * (curLevel - curLevel);
        else
            return curLevel - miniBosses;
        */
    }
}
