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

    // Returns the current level index, skipping minibosses
    public int GetLevelMinusMiniBosses(int curLevel)
    {
        int miniBosses = 0;
        for(int i = 0; i < Mathf.Min(curLevel, progressList.Count); i++)
        {
            if(progressList[i].sceneType == GlobalVars.SceneType.MINIBOSS)
                miniBosses++;
        }
        return curLevel - miniBosses;
    }
}
