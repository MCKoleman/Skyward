using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonRoomList", menuName = "ScriptableObjects/DungeonRoomList", order = 1)]
[System.Serializable]
public class DungeonRoomList : ScriptableObject
{
    [SerializeField]
    private WeightedGameObjectList roomList;

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

            // Get the flag from the room
            uint newFlag = tempRoom.reqFlag;

            // Bitwise and the requirement flags to see which components match. If all components match, add the room to the output
            if((newFlag & reqFlag) == reqFlag)
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
}
