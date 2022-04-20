using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GlobalVars : Singleton<GlobalVars>
{
    [System.Serializable]
    public enum ElementType { DEFAULT, FIRE, ICE, LIGHTNING };
    [System.Serializable]
    public enum AbilityType { DEFAULT = 0, MAGIC_MISSILE = 1, METEOR = 2, ICE_WAVE = 3, LIGHTNING_BOLT = 4 };
    [System.Serializable]
    public enum ContentType { NOTHING, ENEMY, TREASURE, HAZARD, WALL };

    [System.Serializable]
    public enum Direction { MIDDLE = 0, TOP = 1, BOTTOM = 2, RIGHT = 3, LEFT = 4 }

    // NONE = 0_0000_0001,
    // TOP = 0_0000_0010, BOTTOM = 0_0000_0100, RIGHT = 0_0000_1000, LEFT = 0_0001_0000,
    // NOTOP = 0_0010_0000, NOBOTTOM = 0_0100_0000, NORIGHT = 0_1000_0000, NOLEFT = 1_0000_0000
    [System.Serializable]
    public enum RoomReq { 
        NONE = 1, 
        TOP_DOOR = 2,   BOTTOM_DOOR = 4,    RIGHT_DOOR = 8,     LEFT_DOOR = 16, 
        TOP_WALL = 32,  BOTTOM_WALL = 64,   RIGHT_WALL = 128,   LEFT_WALL = 256,
        TOP_OPEN = 512, BOTTOM_OPEN = 1024, RIGHT_OPEN = 2048,  LEFT_OPEN = 4096
    }

    [System.Serializable]
    public enum DungeonTheme { DEFAULT = 0, CAVE = 1, SKY = 2, CASTLE = 3 }
    [SerializeField]
    public enum SceneType { MENU = 0, DUNGEON = 1, MINIBOSS = 2, BOSS = 3 }

    [System.Serializable]
    public enum WallType { DEFAULT = 0, TOP = 1, SIDE = 2, TOP_DOOR = 3, SIDE_DOOR = 4, FLOOR = 5 }

    [System.Serializable]
    public enum SpeakerType { PLAYER, ZIEL, VISAGE, MOM, DAD, VISAGE_1, VISAGE_2, BOTH }

    // Returns whether the room with given flagType requires a special room
    public static bool DoesRequireSpecialReq(uint reqFlag)
    {
        // Open rooms are guaranteed to have a value of 512 or greater
        return reqFlag >= 512;
    }

    // Locks the requirement flag to not allow any variation, ie. if top is not required, don't allow it
    public static uint LockFlagReqs(uint reqFlag)
    {
        uint newFlag = reqFlag;

        // If the TOP bit is 0, set the NOTOP bit to 1
        if ((newFlag & ((int)RoomReq.TOP_DOOR)) == 0)
            newFlag |= ((int)RoomReq.TOP_WALL);

        // If the BOTTOM bit is 0, set the NOBOTTOM bit to 1
        if ((newFlag & ((int)RoomReq.BOTTOM_DOOR)) == 0)
            newFlag |= ((int)RoomReq.BOTTOM_WALL);

        // If the RIGHT bit is 0, set the NORIGHT bit to 1
        if ((newFlag & ((int)RoomReq.RIGHT_DOOR)) == 0)
            newFlag |= ((int)RoomReq.RIGHT_WALL);

        // If the LEFT bit is 0, set the NOLEFT bit to 1
        if ((newFlag & ((int)RoomReq.LEFT_DOOR)) == 0)
            newFlag |= ((int)RoomReq.LEFT_WALL);

        return newFlag;
    }

    // Locks the requirement flag to allow maximum variation, ie. if top is allowed, require it
    public static uint LockInverseFlagReqs(uint reqFlag)
    {
        uint newFlag = reqFlag;

        // If the NOTOP bit is 0, set the TOP bit to 1
        if ((newFlag & ((int)RoomReq.TOP_WALL)) == 0)
            newFlag |= ((int)RoomReq.TOP_DOOR);

        // If the NOBOTTOM bit is 0, set the BOTTOM bit to 1
        if ((newFlag & ((int)RoomReq.BOTTOM_WALL)) == 0)
            newFlag |= ((int)RoomReq.BOTTOM_DOOR);

        // If the NORIGHT bit is 0, set the RIGHT bit to 1
        if ((newFlag & ((int)RoomReq.RIGHT_WALL)) == 0)
            newFlag |= ((int)RoomReq.RIGHT_DOOR);

        // If the NOLEFT bit is 0, set the LEFT bit to 1
        if ((newFlag & ((int)RoomReq.LEFT_WALL)) == 0)
            newFlag |= ((int)RoomReq.LEFT_DOOR);

        return newFlag;
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
        switch (req)
        {
            case RoomReq.TOP_DOOR:
                return RoomReq.TOP_WALL;
            case RoomReq.BOTTOM_DOOR:
                return RoomReq.BOTTOM_WALL;
            case RoomReq.RIGHT_DOOR:
                return RoomReq.RIGHT_WALL;
            case RoomReq.LEFT_DOOR:
                return RoomReq.LEFT_WALL;
            case RoomReq.TOP_WALL:
                return RoomReq.TOP_DOOR;
            case RoomReq.BOTTOM_WALL:
                return RoomReq.BOTTOM_DOOR;
            case RoomReq.RIGHT_WALL:
                return RoomReq.RIGHT_DOOR;
            case RoomReq.LEFT_WALL:
                return RoomReq.LEFT_DOOR;
            case RoomReq.NONE:
            default:
                return RoomReq.NONE;
        }
    }

    // Returns the opposite requirement of the one given
    public static RoomReq GetOppositeReq(RoomReq req)
    {
        switch (req)
        {
            case RoomReq.TOP_DOOR:
                return RoomReq.BOTTOM_DOOR;
            case RoomReq.BOTTOM_DOOR:
                return RoomReq.TOP_DOOR;
            case RoomReq.RIGHT_DOOR:
                return RoomReq.LEFT_DOOR;
            case RoomReq.LEFT_DOOR:
                return RoomReq.RIGHT_DOOR;
            case RoomReq.TOP_WALL:
                return RoomReq.BOTTOM_WALL;
            case RoomReq.BOTTOM_WALL:
                return RoomReq.TOP_WALL;
            case RoomReq.RIGHT_WALL:
                return RoomReq.LEFT_WALL;
            case RoomReq.LEFT_WALL:
                return RoomReq.RIGHT_WALL;
            case RoomReq.NONE:
            default:
                return RoomReq.NONE;
        }
    }
}
