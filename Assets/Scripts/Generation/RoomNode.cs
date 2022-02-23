using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
    public bool shouldSpawn;
    public bool hasSpawned;
    [SerializeField]
    private GlobalVars.RoomReq req;
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
