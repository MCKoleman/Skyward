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
    public Vector2 trBound = Vector2.zero;
    public Vector2 blBound = Vector2.zero;
    public uint reqFlag;
    public int roomNum = 0;
    public GlobalVars.DungeonTheme theme;
    public Vector2Int roomPos;
    public Vector2Int roomSize;
    public bool isRevealed = false;

    void Awake()
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

    // Returns the position of the room
    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    // Returns the size of the room, calculating it if it hasn't been calculated yet
    public Vector2Int GetSize()
    {
        // If the roomSize is null, calculate it
        if (roomSize == Vector2.zero)
            roomSize = CalcSize();

        return roomSize;
    }

    // Calculates and returns the size of the room based on how additional nodes are placed
    private Vector2Int CalcSize()
    {
        float minX = int.MaxValue;
        float minZ = int.MaxValue;
        float maxX = int.MinValue;
        float maxZ = int.MinValue;

        // Find the lowest and highest x and z
        for(int i = 0; i < borderNodes.Count; i++)
        {
            minX = Mathf.Min(borderNodes[i].transform.position.x, minX);
            minZ = Mathf.Min(borderNodes[i].transform.position.z, minZ);
            maxX = Mathf.Max(borderNodes[i].transform.position.x, maxX);
            maxZ = Mathf.Max(borderNodes[i].transform.position.z, maxZ);
        }

        // Set bounds of the room
        blBound = new Vector2(minX, minZ);
        trBound = new Vector2(maxX, maxZ);

        // The size of a room is half the distance between its furthest nodes
        return new Vector2Int(Mathf.FloorToInt(maxX - minX), Mathf.FloorToInt(maxZ - minZ));
    }
}
