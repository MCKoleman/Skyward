using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonRoom : MonoBehaviour
{
    public enum RoomReq { TOP = 1, BOTTOM = 2, RIGHT = 4, LEFT = 8 }

    public List<RoomReq> roomReqs = new List<RoomReq>();
    public uint reqFlag;

    void Start()
    {
        CalcReqFlag();
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
        uint _reqFlag = 0;
        _reqs = _reqs.Distinct().ToList();

        // Add all elements of the room requirements list
        for (int i = 0; i < _reqs.Count; i++)
        {
            _reqFlag += (byte)_reqs[i];
        }

        return _reqFlag;
    }
}
