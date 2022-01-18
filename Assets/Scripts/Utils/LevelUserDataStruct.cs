using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelUserDataStruct
{
    public int collectiblesFound;
    public int bestDeaths;
    public float bestTime;
    public bool hasCleared;
    public char rating;

    public LevelUserDataStruct(bool _cleared)
    {
        collectiblesFound = -1;
        bestDeaths = -1;
        bestTime = -1.0f;
        hasCleared = _cleared;
        rating = 'I';
    }

    public LevelUserDataStruct(int _collectibles, int _deaths, float _time, bool _cleared, char _rating)
    {
        collectiblesFound = _collectibles;
        bestDeaths = _deaths;
        bestTime = _time;
        hasCleared = _cleared;
        rating = _rating;
    }

    /// <summary>
    /// Sets the new best time for this level
    /// </summary>
    /// <param name="_newTime">New time</param>
    /// <returns>Whether the given time is the new best</returns>
    public bool SetBestTime(float _newTime)
    {
        // Set best time to new time if new time is better or if old time is negative
        if (_newTime < bestTime || bestTime <= 0.0f)
        {
            bestTime = _newTime;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets the best deaths for this level
    /// </summary>
    /// <param name="_newDeaths">New Deaths</param>
    /// <returns>Whether the given deaths is the new best</returns>
    public bool SetBestDeaths(int _newDeaths)
    {
        // Set best deaths to new deaths if new deaths is better or if old deaths is negative
        if (_newDeaths < bestDeaths || bestDeaths < 0)
        {
            bestDeaths = _newDeaths;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets the best collectibles found for this level
    /// </summary>
    /// <param name="_newCollectibles">New Collectibles</param>
    /// <returns>Whether the given collectibles is the new best</returns>
    public bool SetBestCollectibles(int _newCollectibles)
    {
        // Set new collectibles if more were found than earlier
        if (_newCollectibles > collectiblesFound)
        {
            collectiblesFound = _newCollectibles;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets the cleared status of the level to the given value (assumed true)
    /// </summary>
    /// <param name="_isCleared">Whether this level was cleared or not</param>
    public void SetCleared(bool _isCleared = true)
    {
        hasCleared = _isCleared;
    }

    /// <summary>
    /// Sets the rating of this level
    /// </summary>
    /// <param name="_rating"></param>
    public void SetRating(char _rating)
    {
        rating = _rating;
    }
}
