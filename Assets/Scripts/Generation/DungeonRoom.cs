using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    // NONE = 0_0000_0001,
    // TOP = 0_0000_0010, BOTTOM = 0_0000_0100, RIGHT = 0_0000_1000, LEFT = 0_0001_0000,
    // NOTOP = 0_0010_0000, NOBOTTOM = 0_0100_0000, NORIGHT = 0_1000_0000, NOLEFT = 1_0000_0000
    //public enum RoomReq { NONE = 1, TOP = 2, BOTTOM = 4, RIGHT = 8, LEFT = 16, NOTOP = 32, NOBOTTOM = 64, NORIGHT = 128, NOLEFT = 256 }

    public List<GlobalVars.RoomReq> roomReqs = new List<GlobalVars.RoomReq>();
    public List<RoomNode> roomNodes = new List<RoomNode>();
    public List<GameObject> borderNodes = new List<GameObject>();
    public uint reqFlag;
    public int roomNum = 0;
    public GlobalVars.DungeonTheme theme;
    public Vector2Int roomPos;
    public Vector2Int roomSize;

    void Start()
    {
        CalcReqFlag();
    }
    
    // Returns all nodes from the list that should spawn
    public List<RoomNode> GetSpawnNodes()
    {
        List<RoomNode> newNodes = new List<RoomNode>();
        for (int i = 0; i < roomNodes.Count; i++)
        {
            // Only add nodes that should spawn
            if(roomNodes[i].shouldSpawn)
                newNodes.Add(roomNodes[i]);
        }
        return newNodes;
    }

    // Calculate the requirements flag from the room requirements, then store it and return it
    public uint CalcReqFlag()
    {
        reqFlag = GlobalVars.CalcReqFlagFromReqs(roomReqs);
        return reqFlag;
    }

    // Calculates and returns the size of the room based on how additional nodes are placed
    public Vector2 CalcSize()
    {
        float minX = int.MaxValue;
        float minZ = int.MaxValue;
        float maxX = int.MinValue;
        float maxZ = int.MinValue;

        // Find the lowest and highest x and z
        for(int i = 0; i < borderNodes.Count; i++)
        {
            if (roomNodes[i].transform.position.x < minX)
                minX = roomNodes[i].transform.position.x;

            if (roomNodes[i].transform.position.x > maxX)
                maxX = roomNodes[i].transform.position.x;

            if (roomNodes[i].transform.position.z < minZ)
                minZ = roomNodes[i].transform.position.z;

            if (roomNodes[i].transform.position.z > maxZ)
                maxZ = roomNodes[i].transform.position.z;
        }

        // The size of a room is half the distance between its furthest nodes
        return new Vector2(maxX - minX, maxZ - minZ);
    }
}
