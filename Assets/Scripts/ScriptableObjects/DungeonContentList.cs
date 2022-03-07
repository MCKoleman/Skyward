using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonContentList", menuName = "ScriptableObjects/DungeonContentList", order = 1)]
[System.Serializable]
public class DungeonContentList : ScriptableObject
{
    [Header("Content lists")]
    [SerializeField]
    private WeightedGameObjectList enemyList = new WeightedGameObjectList();
    [SerializeField]
    private WeightedGameObjectList treasureList = new WeightedGameObjectList();
    [SerializeField]
    private WeightedGameObjectList hazardList = new WeightedGameObjectList();
    [SerializeField]
    private WeightedGameObjectList wallObjList = new WeightedGameObjectList();

    [Header("List weights")]
    [SerializeField]
    private int nothingWeight;
    [SerializeField]
    private int enemyWeight;
    [SerializeField]
    private int treasureWeight;
    [SerializeField]
    private int hazardWeight;
    [SerializeField]
    private float wallObjWeight = 0.3f;

    [Header("Base weights")]
    [SerializeField, Range(0f, 1f)]
    private float baseNothingWeight = 1f;
    [SerializeField, Range(0f, 1f)]
    private float baseEnemyWeight = 0.1f;
    [SerializeField, Range(0f, 1f)]
    private float baseTreasureWeight = 0.1f;
    [SerializeField, Range(0f, 1f)]
    private float baseHazardWeight = 0.1f;
    [SerializeField, Range(0f, 1f)]
    private float baseWallObjWeight = 0.1f;

    [Header("Room percent weights")]
    [SerializeField]
    private float nothingMod;
    [SerializeField]
    private float enemyMod;
    [SerializeField]
    private float treasureMod;
    [SerializeField]
    private float hazardMod;

    // Returns a random gameObject from all the lists based on the given random values
    public GameObject GetRandomContent(float roomPercent, ContentNode.NodePlace place)
    {
        // Change content generation based on node type
        switch(place)
        {
            case ContentNode.NodePlace.CENTER:
                return GetRandomContent(roomPercent);
            case ContentNode.NodePlace.EDGE:
                return GetRandomContent(roomPercent);
            case ContentNode.NodePlace.CORNER:
                return GetRandomContent(roomPercent);
            case ContentNode.NodePlace.WALL:
                return GetRandomWallContent(roomPercent);
            case ContentNode.NodePlace.NORMAL:
                return GetRandomContent(roomPercent);
            default:
                return null;
        }
    }

    // Gets a random wall object
    private GameObject GetRandomWallContent(float roomPercent)
    {
        float rand = Random.Range(0.0f, 1.0f);
        if (rand < baseWallObjWeight * Mathf.Pow(1 + wallObjWeight, 1 + roomPercent))
            return wallObjList.GetRandomObject();
        else
            return null;
    }

    // Gets random content with the given room percent
    private GameObject GetRandomContent(float roomPercent)
    {
        // Find modifiers
        float kN = Mathf.Clamp((baseNothingWeight + roomPercent * nothingMod), 0.0f, 2.0f) * nothingWeight;
        float kE = Mathf.Clamp((baseEnemyWeight + roomPercent * enemyMod), 0.0f, 1.0f) * enemyWeight;
        float kT = Mathf.Clamp((baseTreasureWeight + roomPercent * treasureMod), 0.0f, 1.0f) * treasureWeight;
        float kH = Mathf.Clamp((baseHazardWeight + roomPercent * hazardMod), 0.0f, 1.0f) * hazardWeight;

        float rand = Random.Range(0, kN + kE + kT + kH);

        // Get random enemy
        if (rand < kE)
        {
            return enemyList.GetRandomObject();
        }
        rand -= kE;

        // Get random treasure
        if (rand < kT)
        {
            return treasureList.GetRandomObject();
        }
        rand -= kT;

        // Get random hazard
        if (rand < kH)
        {
            return hazardList.GetRandomObject();
        }

        // Spawn nothing
        return null;
    }
}
