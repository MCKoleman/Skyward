using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MinimapNode
{
    public DungeonRoom room;
    public bool isDiscovered;


    public MinimapNode(DungeonRoom _room) { room = _room; isDiscovered = false; }
    public void Discover() { isDiscovered = true; }
    public List<GlobalVars.RoomReq> GetRoomReqs() { return room.roomReqs; }
}
