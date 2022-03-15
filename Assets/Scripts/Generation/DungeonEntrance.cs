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
        if (!other.CompareTag("Player"))
            return;

        // Set the current room to this room
        DungeonManager.Instance.SetCurrentRoom(room);
    }
}
