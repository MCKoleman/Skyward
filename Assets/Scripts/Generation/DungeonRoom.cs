using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonRoom : MonoBehaviour
{
    // NONE = 0_0000_0001,
    // TOP = 0_0000_0010, BOTTOM = 0_0000_0100, RIGHT = 0_0000_1000, LEFT = 0_0001_0000,
    // NOTOP = 0_0010_0000, NOBOTTOM = 0_0100_0000, NORIGHT = 0_1000_0000, NOLEFT = 1_0000_0000
    //public enum RoomReq { NONE = 1, TOP = 2, BOTTOM = 4, RIGHT = 8, LEFT = 16, NOTOP = 32, NOBOTTOM = 64, NORIGHT = 128, NOLEFT = 256 }

    public List<RoomReq> roomReqs = new List<RoomReq>();
    public List<RoomNode> roomNodes = new List<RoomNode>();
    public uint reqFlag;

    void Start()
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

    // Calculate the requirements flag from the room requirements, then store it and return it
    public uint CalcReqFlag()
    {
        reqFlag = CalcReqFlagFromReqs(roomReqs);
        return reqFlag;
    }

    // Calculate the requirements flag from the given requirements
    public static uint CalcReqFlagFromReqs(List<RoomReq> _reqs)
    {
        // Make sure all elements are unique
        uint _reqFlag = 0;
        _reqs = _reqs.Distinct().ToList();

        // Add all elements of the room requirements list
        for (int i = 0; i < _reqs.Count; i++)
        {
            _reqFlag += (uint)_reqs[i];
        }

        return _reqFlag;
    }

    // Returns the negated requirement of the one given
    public static RoomReq GetNegatedReq(RoomReq req)
    {
        switch(req)
        {
            case RoomReq.TOP:
                return RoomReq.NOTOP;
            case RoomReq.BOTTOM:
                return RoomReq.NOBOTTOM;
            case RoomReq.RIGHT:
                return RoomReq.NORIGHT;
            case RoomReq.LEFT:
                return RoomReq.NOLEFT;
            case RoomReq.NOTOP:
                return RoomReq.TOP;
            case RoomReq.NOBOTTOM:
                return RoomReq.BOTTOM;
            case RoomReq.NORIGHT:
                return RoomReq.RIGHT;
            case RoomReq.NOLEFT:
                return RoomReq.LEFT;
            case RoomReq.NONE:
            default:
                return RoomReq.NONE;
        }
    }

    // Returns the opposite requirement of the one given
    public static RoomReq GetOppositeReq(RoomReq req)
    {
        switch(req)
        {
            case RoomReq.TOP:
                return RoomReq.BOTTOM;
            case RoomReq.BOTTOM:
                return RoomReq.TOP;
            case RoomReq.RIGHT:
                return RoomReq.LEFT;
            case RoomReq.LEFT:
                return RoomReq.RIGHT;
            case RoomReq.NOTOP:
                return RoomReq.NOBOTTOM;
            case RoomReq.NOBOTTOM:
                return RoomReq.NOTOP;
            case RoomReq.NORIGHT:
                return RoomReq.NOLEFT;
            case RoomReq.NOLEFT:
                return RoomReq.NORIGHT;
            case RoomReq.NONE:
            default:
                return RoomReq.NONE;
        }
    }
}
