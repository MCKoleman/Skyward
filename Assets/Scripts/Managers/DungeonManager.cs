using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    [SerializeField]
    private DungeonRoomList roomList;

    // Should only be called by GameManager. Initializes the singleton
    public void Init()
    {
        List<DungeonRoom.RoomReq> reqs = new List<DungeonRoom.RoomReq>();
        reqs.Add(DungeonRoom.RoomReq.TOP);
        reqs.Add(DungeonRoom.RoomReq.BOTTOM);
        uint reqFlag = DungeonRoom.CalcReqFlagFromReqs(reqs);
        Print.Log($"Found flag: [{reqFlag}]");

        for(int i = 0; i < 25; i++)
        {
            Print.Log($"Found room: [{roomList.GetRandomRoomByFlag(reqFlag)}]");
        }
    }
}
