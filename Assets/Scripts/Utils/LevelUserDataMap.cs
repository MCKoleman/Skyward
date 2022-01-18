using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelUserDataMap
{
    [SerializeField]
    private Dictionary<int, LevelUserDataStruct> data = new Dictionary<int, LevelUserDataStruct>();

    public int Count { get { return data.Count; } }

    public LevelUserDataStruct this[int levelID]
    {
        get { return data[levelID]; }
        set { data[levelID] = value; }
    }

    /// <summary>
    /// Returns the level with the given ID
    /// </summary>
    /// <param name="levelID">Level ID to search for</param>
    /// <returns></returns>
    public LevelUserDataStruct GetLevel(int levelID)
    {
        // If the map has the level, return it
        if (data.ContainsKey(levelID))
            return data[levelID];
        // If the map doesn't have the level, make a new one
        else
            return CreateNewLevel(levelID);
    }

    /// <summary>
    /// Creates a new level with given build index and adds it to the map.
    /// </summary>
    /// <param name="levelID">Level ID of new level</param>
    /// <returns>Newly created level or old one if it exists</returns>
    public LevelUserDataStruct CreateNewLevel(int levelID)
    {
        // Only create a new level if the map doesn't already have it
        if(!data.ContainsKey(levelID))
        {
            Print.LogWarning($"Encountered level that does not exist yet, [{levelID}]. Creating it...");
            LevelUserDataStruct tempLevel = new LevelUserDataStruct(false);
            data.Add(levelID, tempLevel);
            return tempLevel;
        }
        // Otherwise return the existing level
        else
        {
            return data[levelID];
        }
    }

    /// <summary>
    /// Returns the latest level that has been cleared (if there are gaps, returns the last level before the gap)
    /// </summary>
    /// <returns>Latest level that has been cleared, or -1 if none have been cleared</returns>
    public int GetLatestClearedLevel()
    {
        // Check completion of each level
        int tempKey = -1;
        foreach(var elem in data)
        {
            // If a level has been cleared and their index is higher than the previous highest, save it
            if (elem.Value.hasCleared && elem.Key > tempKey)
                tempKey = elem.Key;
        }

        // Return highest cleared levelID
        return tempKey;
    }

    public void Add(int levelID, LevelUserDataStruct level)
    {
        data.Add(levelID, level);
    }

    public bool Remove(int levelID)
    {
        return data.Remove(levelID);
    }

    public void Clear()
    {
        data.Clear();
    }

    public LevelUserDataMap DeepCopy()
    {
        LevelUserDataMap newMap = new LevelUserDataMap();

        // Copy each value from the old map
        foreach(var elem in data)
        {
            newMap.Add(elem.Key, elem.Value);
        }

        return newMap;
    }

    public bool TryGetValue(int levelID, out LevelUserDataStruct level)
    {
        return data.TryGetValue(levelID, out level);
    }

    public bool ContainsLevel(int levelID)
    {
        return data.ContainsKey(levelID);
    }

    public bool ContainsLevel(LevelUserDataStruct level)
    {
        return data.ContainsValue(level);
    }

    /// <summary>
    /// Returns a reference to the stored data map
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, LevelUserDataStruct> GetDataMap() { return data; }
}
