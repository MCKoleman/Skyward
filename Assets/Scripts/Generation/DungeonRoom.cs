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

    public List<GlobalVars.RoomReq> roomReqs = new List<GlobalVars.RoomReq>();
    public List<RoomNode> roomNodes = new List<RoomNode>();
    public uint reqFlag;
    public GlobalVars.DungeonTheme theme;

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
    public static uint CalcReqFlagFromReqs(List<GlobalVars.RoomReq> _reqs)
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
    public static GlobalVars.RoomReq GetNegatedReq(GlobalVars.RoomReq req)
    {
        switch(req)
        {
            case GlobalVars.RoomReq.TOP:
                return GlobalVars.RoomReq.NOTOP;
            case GlobalVars.RoomReq.BOTTOM:
                return GlobalVars.RoomReq.NOBOTTOM;
            case GlobalVars.RoomReq.RIGHT:
                return GlobalVars.RoomReq.NORIGHT;
            case GlobalVars.RoomReq.LEFT:
                return GlobalVars.RoomReq.NOLEFT;
            case GlobalVars.RoomReq.NOTOP:
                return GlobalVars.RoomReq.TOP;
            case GlobalVars.RoomReq.NOBOTTOM:
                return GlobalVars.RoomReq.BOTTOM;
            case GlobalVars.RoomReq.NORIGHT:
                return GlobalVars.RoomReq.RIGHT;
            case GlobalVars.RoomReq.NOLEFT:
                return GlobalVars.RoomReq.LEFT;
            case GlobalVars.RoomReq.NONE:
            default:
                return GlobalVars.RoomReq.NONE;
        }
    }

    // Returns the opposite requirement of the one given
    public static GlobalVars.RoomReq GetOppositeReq(GlobalVars.RoomReq req)
    {
        switch(req)
        {
            case GlobalVars.RoomReq.TOP:
                return GlobalVars.RoomReq.BOTTOM;
            case GlobalVars.RoomReq.BOTTOM:
                return GlobalVars.RoomReq.TOP;
            case GlobalVars.RoomReq.RIGHT:
                return GlobalVars.RoomReq.LEFT;
            case GlobalVars.RoomReq.LEFT:
                return GlobalVars.RoomReq.RIGHT;
            case GlobalVars.RoomReq.NOTOP:
                return GlobalVars.RoomReq.NOBOTTOM;
            case GlobalVars.RoomReq.NOBOTTOM:
                return GlobalVars.RoomReq.NOTOP;
            case GlobalVars.RoomReq.NORIGHT:
                return GlobalVars.RoomReq.NOLEFT;
            case GlobalVars.RoomReq.NOLEFT:
                return GlobalVars.RoomReq.NORIGHT;
            case GlobalVars.RoomReq.NONE:
            default:
                return GlobalVars.RoomReq.NONE;
        }
    }
}
