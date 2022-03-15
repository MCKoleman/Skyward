using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GlobalVars : Singleton<GlobalVars>
{
    [System.Serializable]
    public enum ElementType { DEFAULT, FIRE, ICE, LIGHTNING };

    [System.Serializable]
    public enum Direction { MIDDLE = 0, TOP = 1, BOTTOM = 2, RIGHT = 3, LEFT = 4 }

    // NONE = 0_0000_0001,
    // TOP = 0_0000_0010, BOTTOM = 0_0000_0100, RIGHT = 0_0000_1000, LEFT = 0_0001_0000,
    // NOTOP = 0_0010_0000, NOBOTTOM = 0_0100_0000, NORIGHT = 0_1000_0000, NOLEFT = 1_0000_0000
    [System.Serializable]
    public enum RoomReq { NONE = 1, TOP = 2, BOTTOM = 4, RIGHT = 8, LEFT = 16, NOTOP = 32, NOBOTTOM = 64, NORIGHT = 128, NOLEFT = 256 }

    [System.Serializable]
    public enum DungeonTheme { DEFAULT = 0, CAVE = 1, SKY = 2, CASTLE = 3 }
    [SerializeField]
    public enum SceneType { MENU = 0, DUNGEON = 1, BOSS = 2 }

    [System.Serializable]
    public enum WallType { DEFAULT = 0, TOP = 1, SIDE = 2, TOP_DOOR = 3, SIDE_DOOR = 4, FLOOR = 5 }

    [System.Serializable]
    public enum SpeakerType { PLAYER, ZEIL, VISAGE, MOM, DAD }

    // Locks the requirement flag to not allow any variation, ie. if top is not required, don't allow it
    public static uint LockFlagReqs(uint reqFlag)
    {
        uint newFlag = reqFlag;

        // If the TOP bit is 0, set the NOTOP bit to 1
        if ((newFlag & ((int)RoomReq.TOP)) == 0)
            newFlag |= ((int)RoomReq.NOTOP);

        // If the BOTTOM bit is 0, set the NOBOTTOM bit to 1
        if ((newFlag & ((int)RoomReq.BOTTOM)) == 0)
            newFlag |= ((int)RoomReq.NOBOTTOM);

        // If the RIGHT bit is 0, set the NORIGHT bit to 1
        if ((newFlag & ((int)RoomReq.RIGHT)) == 0)
            newFlag |= ((int)RoomReq.NORIGHT);

        // If the LEFT bit is 0, set the NOLEFT bit to 1
        if ((newFlag & ((int)RoomReq.LEFT)) == 0)
            newFlag |= ((int)RoomReq.NOLEFT);

        return newFlag;
    }

    // Locks the requirement flag to allow maximum variation, ie. if top is allowed, require it
    public static uint LockInverseFlagReqs(uint reqFlag)
    {
        uint newFlag = reqFlag;

        // If the NOTOP bit is 0, set the TOP bit to 1
        if ((newFlag & ((int)RoomReq.NOTOP)) == 0)
            newFlag |= ((int)RoomReq.TOP);

        // If the NOBOTTOM bit is 0, set the BOTTOM bit to 1
        if ((newFlag & ((int)RoomReq.NOBOTTOM)) == 0)
            newFlag |= ((int)RoomReq.BOTTOM);

        // If the NORIGHT bit is 0, set the RIGHT bit to 1
        if ((newFlag & ((int)RoomReq.NORIGHT)) == 0)
            newFlag |= ((int)RoomReq.RIGHT);

        // If the NOLEFT bit is 0, set the LEFT bit to 1
        if ((newFlag & ((int)RoomReq.NOLEFT)) == 0)
            newFlag |= ((int)RoomReq.LEFT);

        return newFlag;
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
        switch (req)
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
        switch (req)
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
