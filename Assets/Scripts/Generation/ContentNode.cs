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
        DungeonManager.Instance.SpawnContent(this);
        DestroySelf();
    }

    // Destroys the node, allowing for additional cleanup
    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public DungeonRoom GetParentRoom() { return parentRoom; }
}
