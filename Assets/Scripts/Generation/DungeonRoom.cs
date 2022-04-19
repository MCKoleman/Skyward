using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    // NONE = 0_0000_0001,
    // TOP = 0_0000_0010, BOTTOM = 0_0000_0100, RIGHT = 0_0000_1000, LEFT = 0_0001_0000,
    // NOTOP = 0_0010_0000, NOBOTTOM = 0_0100_0000, NORIGHT = 0_1000_0000, NOLEFT = 1_0000_0000
    //public enum RoomReq { NONE = 1, TOP = 2, BOTTOM = 4, RIGHT = 8, LEFT = 16, NOTOP = 32, NOBOTTOM = 64, NORIGHT = 128, NOLEFT = 256 }

    [Header("Spawn lists")]
    public List<GlobalVars.RoomReq> roomReqs = new List<GlobalVars.RoomReq>();
    public List<RoomNode> roomNodes = new List<RoomNode>();
    public List<GameObject> borderNodes = new List<GameObject>();

    [Header("Spawn info")]
    public uint reqFlag;
    public int roomNum = 0;
    public GlobalVars.DungeonTheme theme;
    public bool isCombinedRoom = false;

    [Header("Size info")]
    public Vector2 trBound = Vector2.zero;
    public Vector2 blBound = Vector2.zero;
    public Vector2Int roomPos;
    public Vector2Int roomSize;

    [Header("Runtime info")]
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

    // Reveals the fog of war for this room
    public virtual void RevealFogOfWar()
    {
        isRevealed = true;
        FogOfWarClear tempFog = this.GetComponentInChildren<FogOfWarClear>();
        if (tempFog != null)
            tempFog.Reveal();
    }


    // Calculate the requirements flag from the room requirements, then store it and return it
    public uint CalcReqFlag()
    {
        reqFlag = GlobalVars.CalcReqFlagFromReqs(roomReqs);
        return reqFlag;
    }

    // Returns the position of the room
    public virtual Vector3 GetPosition()
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
    protected virtual Vector2Int CalcSize()
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

    // Checks combination of this room with its border rooms
    public void CheckCombination()
    {
        // Don't evaluate rooms for combination that don't need combining
        if (!isCombinedRoom)
            return;

        //Debug.Log($"Checking room [{this.gameObject}] for combination:");

        // Search all bordered nodes for combining
        for(int i = 0; i < roomNodes.Count; i++)
        {
            // Don't check middle nodes
            if (roomNodes[i].GetOriginalReq() == GlobalVars.RoomReq.NONE)
                continue;

            //Debug.Log($"Checking node [{i}: {roomNodes[i]}] for combination. Has flag " +
            //    $"[{roomNodes[i].ReqFlag}], which [{(GlobalVars.DoesRequireSpecialReq(roomNodes[i].ReqFlag) ? "requires" : "does not require")}] open space " +
            //    $"and [{(roomNodes[i].hasSpawned ? "has" : "has not")}] spawned yet.");

            // If a border node has spawned a special room, check for combining
            if (roomNodes[i].hasSpawned && GlobalVars.DoesRequireSpecialReq(roomNodes[i].ReqFlag)) {
                CombineRooms(DungeonManager.Instance.GetRoomWithKey(roomNodes[i].GetKey()));
            }
        }
    }

    // Combines this room with the given room
    public virtual void CombineRooms(DungeonRoom otherRoom)
    {
        //Debug.Log($"Combine rooms called between [{this.gameObject}] and [{otherRoom.gameObject}]");

        // Don't combine rooms that don't want to be combined
        if (otherRoom == null || !isCombinedRoom || !otherRoom.isCombinedRoom)
            return;

        //Debug.Log("Combine rooms passed guarding");

        // If the other room is already part of a combined room, add this room to that room
        if(otherRoom.IsPartOfCombinedRoom())
        {
            CombinedDungeonRoom combinedRoom = otherRoom.GetComponentInParent<CombinedDungeonRoom>();
            combinedRoom.AddLinkedRoom(this);
            combinedRoom.EvaluateRoom();
            return;
        }

        // Spawn a new room to combine under
        GameObject tempRoom = Instantiate(PrefabManager.Instance.combinedRoomPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
        CombinedDungeonRoom newRoom = tempRoom.GetComponent<CombinedDungeonRoom>();
        newRoom.AddLinkedRoom(this);
        newRoom.AddLinkedRoom(otherRoom);
        newRoom.EvaluateRoom();
    }

    // Returns the position of this object as a string
    public string GetKey()
    {
        return ((int)this.transform.position.x).ToString() +
            ((int)this.transform.position.y).ToString() +
            ((int)this.transform.position.z).ToString();
    }

    // Returns whether this room is a part of a combined room
    public virtual bool IsPartOfCombinedRoom()
    {
        return this.GetComponentInParent<CombinedDungeonRoom>() != null;
    }

    // A regular dungeon room cannot be a combined room
    public virtual bool IsCombinedRoom() { return false; }
}
