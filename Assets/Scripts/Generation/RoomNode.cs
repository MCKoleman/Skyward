using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// NONE = 0_0000_0001,
// TOP = 0_0000_0010, BOTTOM = 0_0000_0100, RIGHT = 0_0000_1000, LEFT = 0_0001_0000,
// NOTOP = 0_0010_0000, NOBOTTOM = 0_0100_0000, NORIGHT = 0_1000_0000, NOLEFT = 1_0000_0000
[System.Serializable]
public enum RoomReq { NONE = 1, TOP = 2, BOTTOM = 4, RIGHT = 8, LEFT = 16, NOTOP = 32, NOBOTTOM = 64, NORIGHT = 128, NOLEFT = 256 }

public class RoomNode : MonoBehaviour
{
    public bool shouldSpawn;
    public bool hasSpawned;
    [SerializeField]
    private RoomReq req;
    public uint reqFlag { get; private set; }

    private void OnEnable()
    {
        reqFlag = (uint)req;
        hasSpawned = false;
    }

    // Combines this node with the given node
    public void CombineNodes(RoomNode other)
    {
        if(other != null)
        {
            // Bitwise OR both flags to keep all reqs of both
            reqFlag |= other.reqFlag;
            // If either node wants to spawn, this should too
            shouldSpawn = other.shouldSpawn || shouldSpawn;
            // If either node has spawned, don't try spawning again
            hasSpawned = other.hasSpawned || hasSpawned;
        }
    }

    // Returns the position of this object as a string
    public string GetKey()
    {
        return ((int)this.transform.position.x).ToString() + 
            ((int)this.transform.position.y).ToString() + 
            ((int)this.transform.position.z).ToString();
    }
}
