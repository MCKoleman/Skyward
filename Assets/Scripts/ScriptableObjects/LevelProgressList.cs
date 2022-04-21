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
        return progressList[curLevel];
    }

    // Returns the desired sceneType of the next scene. If the scene progression is over, the next scene will be the main menu
    public GlobalVars.SceneType GetNextSceneType(int curLevel)
    {
        // If the next level still exists in the progression, return it
        if (curLevel + 1 < progressList.Count)
            return progressList[curLevel + 1].sceneType;
        // Otherwise return menu
        else
            return GlobalVars.SceneType.MENU;
    }

    // Returns the current level index, skipping minibosses
    public int GetLevelMinusMiniBosses(int curLevel)
    {
        int miniBosses = 0;
        for(int i = 0; i < Mathf.Min(curLevel+1, progressList.Count); i++)
        {
            if(progressList[i].sceneType == GlobalVars.SceneType.MINIBOSS)
                miniBosses++;
        }
        return curLevel - miniBosses;
    }
}
