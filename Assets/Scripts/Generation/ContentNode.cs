using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentNode : MonoBehaviour
{
    public enum NodePlace { NORMAL = 0, EDGE = 1, CORNER = 2, CENTER = 3, WALL = 4}

    public NodePlace nodePlace;
    private DungeonRoom parentRoom;

    void Start()
    {
        parentRoom = GetComponentInParent<DungeonRoom>();

        // Don't spawn content in the first or last room
        if ((parentRoom.roomNum >= 1 && parentRoom.roomNum < DungeonManager.Instance.GetNumRooms()-1) || nodePlace == NodePlace.WALL)
            DungeonManager.Instance.SpawnContent(this);
        // Spawn the entrance in the center of the first room
        else if(nodePlace == NodePlace.CENTER)
            Instantiate(PrefabManager.Instance.entrancePrefab, Vector3.zero, Quaternion.identity, PrefabManager.Instance.levelHolder);

        DestroySelf();
    }

    // Destroys the node, allowing for additional cleanup
    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public DungeonRoom GetParentRoom() { return parentRoom; }
}
