using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    private DungeonRoom room;

    private void Awake()
    {
        room = this.GetComponentInParent<DungeonRoom>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore all collisions except the player
        if (!other.CompareTag("Player") || other.isTrigger)
            return;

        // Cast room upwards if needed
        if (room.IsPartOfCombinedRoom() && !room.IsCombinedRoom())
            room = room.GetComponentInParent<CombinedDungeonRoom>();

        // Set the current room to this room
        DungeonManager.Instance.SetCurrentRoom(room);
    }
}
