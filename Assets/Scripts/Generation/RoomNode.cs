using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
    public bool shouldSpawn;
    public bool hasSpawned;
    [SerializeField]
    private GlobalVars.RoomReq req;
    [SerializeField]
    private uint m_reqFlag;
    public uint ReqFlag { get { return m_reqFlag; } private set { m_reqFlag = value; } }

    private void OnEnable()
    {
        ReqFlag = (uint)req;
        hasSpawned = false;
    }

    // Combines this node with the given node
    public void CombineNodes(RoomNode other)
    {
        // Don't combine will null
        if (other == null)
            return;

        // Bitwise OR both flags to keep all reqs of both
        ReqFlag |= other.ReqFlag;
        // If either node wants to spawn, this should too
        shouldSpawn = other.shouldSpawn || shouldSpawn;
        // If either node has spawned, don't try spawning again
        hasSpawned = other.hasSpawned || hasSpawned;

        // Update info of the other to match this
        other.ReqFlag = this.ReqFlag;
        other.shouldSpawn = this.shouldSpawn;
        other.hasSpawned = this.hasSpawned;
    }

    // Returns the position of this object as a string
    public string GetKey()
    {
        return ((int)this.transform.position.x).ToString() + 
            ((int)this.transform.position.y).ToString() + 
            ((int)this.transform.position.z).ToString();
    }

    // Returns the original requirement of this room node
    public GlobalVars.RoomReq GetOriginalReq() { return req; }
}
