using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinedDungeonRoom : DungeonRoom
{
    [SerializeField]
    protected List<DungeonRoom> linkedRooms = new List<DungeonRoom>();

    public override void RevealFogOfWar()
    {
        // Reveal fog of war in all linked rooms
        for (int i = 0; i < linkedRooms.Count; i++)
        {
            linkedRooms[i].isRevealed = true;
            FogOfWarClear tempFog = linkedRooms[i].GetComponentInChildren<FogOfWarClear>();
            if (tempFog != null)
                tempFog.Reveal();
        }
    }

    // Adds the given room to the list of linked rooms of this room
    public void AddLinkedRoom(DungeonRoom newRoom)
    {
        newRoom.transform.SetParent(this.transform, true);
        linkedRooms.Add(newRoom);
    }

    // Combines this room with the given room
    public override void CombineRooms(DungeonRoom otherRoom)
    {
        otherRoom.transform.SetParent(this.transform, true);
        EvaluateRoom();
    }

    // Evaluates border conditions of this room
    public void EvaluateRoom()
    {
        CalcSize();
    }

    // Returns the position of this room
    public override Vector3 GetPosition()
    {
        // If requesting invalid size, recalculate it
        if (blBound == Vector2.zero || trBound == Vector2.zero)
            CalcSize();
        return new Vector3((blBound.x + trBound.x) * 0.5f, 0.0f, (blBound.y + trBound.y) * 0.5f);
    }

    // Calculates the size of this large room
    protected override Vector2Int CalcSize()
    {
        float minX = int.MaxValue;
        float minZ = int.MaxValue;
        float maxX = int.MinValue;
        float maxZ = int.MinValue;

        // Loop through all rooms to find bounds
        for(int j = 0; j < linkedRooms.Count; j++)
        {
            DungeonRoom tempRoom = linkedRooms[j];
            // Find the lowest and highest x and z
            for (int i = 0; i < tempRoom.borderNodes.Count; i++)
            {
                minX = Mathf.Min(tempRoom.borderNodes[i].transform.position.x, minX);
                minZ = Mathf.Min(tempRoom.borderNodes[i].transform.position.z, minZ);
                maxX = Mathf.Max(tempRoom.borderNodes[i].transform.position.x, maxX);
                maxZ = Mathf.Max(tempRoom.borderNodes[i].transform.position.z, maxZ);
            }
        }

        // Set bounds of the room
        blBound = new Vector2(minX, minZ);
        trBound = new Vector2(maxX, maxZ);

        // The size of a room is half the distance between its furthest nodes
        return new Vector2Int(Mathf.FloorToInt(maxX - minX), Mathf.FloorToInt(maxZ - minZ));
    }

    // Combined rooms are always part of a combined room
    public override bool IsPartOfCombinedRoom() { return true; }

    // A combined room is always a combined room
    public override bool IsCombinedRoom() { return true; }
}
