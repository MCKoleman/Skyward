using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonRoomList", menuName = "ScriptableObjects/DungeonRoomList", order = 1)]
[System.Serializable]
public class DungeonRoomList : ScriptableObject
{
    [SerializeField]
    private WeightedGameObjectList roomList = new WeightedGameObjectList();
    [SerializeField, Range(1, 200), Tooltip("The minimum number of rooms to be spawned. This might not be met if rng fails due to conflicting door paths.")]
    private int minRoomCount = 1;
    [SerializeField, Range(1, 200), Tooltip("The preferred number of rooms to be spawned. The algorithm will aim for exactly this many rooms, going slightly under or over to properly close off all exits.")]
    private int prefRoomCount = 10;
    [SerializeField, Range(1, 200), Tooltip("The maximum number of rooms to be spawned. To close off all exits in a dungeon this number may be slightly exceeded.")]
    private int maxRoomCount = 50;

    // Returns all rooms from the roomList that meet the given requirement
    public WeightedGameObjectList GetRoomsByReq(uint reqFlag)
    {
        WeightedGameObjectList newRooms = new WeightedGameObjectList();
        
        // Find all rooms that meet the requirements flag
        for (int i = 0; i < roomList.Count(); i++)
        {
            // If the object in the list is not a room, skip it
            DungeonRoom tempRoom = roomList.GetObject(i).GetComponent<DungeonRoom>();
            if (tempRoom == null)
                continue;

            // If the room does not have a valid flag yet, calculate it
            if (tempRoom.reqFlag == 0)
                tempRoom.CalcReqFlag();

            // Bitwise and the requirement flags to see which components match. If all components match, add the room to the output
            if((tempRoom.reqFlag & reqFlag) == reqFlag)
            {
                //Print.Log($"Bitwise checking [{newFlag}] vs [{reqFlag}]. Result: [{newFlag & reqFlag}]");
                newRooms.Add(roomList.Get(i));
            }
        }

        return newRooms;
    }

    // Find a random room that meets all the requirements of the given flag
    public GameObject GetRandomRoomByFlag(uint reqFlag)
    {
        return GetRoomsByReq(reqFlag).GetRandomObject();
    }

    // Find a random room that meets all the requirements of the given flag with inverted probabilities (higher chance of more complex rooms)
    public GameObject GetInverseRandomRoomByFlag(uint reqFlag)
    {
        return GetRoomsByReq(reqFlag).GetInverseRandomObject();
    }

    // Returns the maximum room count of dungeons
    public int GetMaxRoomCount() { return maxRoomCount; }

    // Returns the minimum room count of dungeons
    public int GetMinRoomCount() { return minRoomCount; }

    // Returns the preferred room count of dungeons
    public int GetPrefRoomCount() { return prefRoomCount; }
}
