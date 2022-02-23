using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    private DungeonRoom parentRoom;
    [SerializeField]
    private GlobalVars.WallType type;

    private void Start()
    {
        parentRoom = this.transform.GetComponentInParent<DungeonRoom>();
        SpawnWall();
    }

    // Spawns the correct wall type at this position
    private void SpawnWall()
    {
        GameObject tempWall = PrefabManager.Instance.GetWallObject(type, parentRoom.theme);
        if (tempWall != null)
        {
            Instantiate(tempWall, this.transform.position, Quaternion.identity, this.transform);
        }
    }
}
