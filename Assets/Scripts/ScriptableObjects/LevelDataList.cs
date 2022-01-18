using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataList", menuName = "ScriptableObjects/LevelDataList", order = 1)]
[System.Serializable]
public class LevelDataList : ScriptableObject
{
    [SerializeField]
    private List<LevelInfoStruct> data;

    public int Count { get { return data.Count; } }

    public LevelInfoStruct this[int levelID]
    {
        get { return GetLevelByID(levelID); }
        set { SetLevelByID(levelID, value); }
    }

    /// <summary>
    /// Returns the level at the given ID
    /// </summary>
    /// <param name="levelID">Level ID to get</param>
    /// <returns></returns>
    public LevelInfoStruct GetLevelByID(int levelID)
    {
        // If the level with the given ID is in the expected location, return it
        if (IsIndexConsistent(levelID))
            return data[levelID - 1];

        // Otherwise, search for the ID
        for(int i = 0; i < data.Count; i++)
        {
            if (data[i].levelID == levelID)
                return data[i];
        }

        // If no ID is found, return first index
        return data[0];
    }

    /// <summary>
    /// Sets the level at the given ID
    /// </summary>
    /// <param name="levelID">Level ID to set</param>
    /// <param name="level">Level to set the ID to</param>
    public void SetLevelByID(int levelID, LevelInfoStruct level)
    {
        // If the level with the given ID is in the expected location, set it
        if (IsIndexConsistent(levelID))
        {
            data[levelID - 1] = level;
        }

        // Otherwise, search for the ID
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].levelID == levelID)
            {
                data[i] = level;
                return;
            }
        }
    }

    /// <summary>
    /// Checks whether an index in the list is consistent with it's supposed placement (index n holds levelID n+1)
    /// </summary>
    /// <param name="levelID">LevelID to check</param>
    /// <returns></returns>
    public bool IsIndexConsistent(int levelID)
    {
        return (levelID > 0 && data[levelID - 1].levelID == levelID);
    }

    /// <summary>
    /// Returns the data object
    /// </summary>
    /// <returns></returns>
    public List<LevelInfoStruct> GetData() { return data; }
}
