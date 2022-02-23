using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [System.Serializable]
    public enum WallType { DEFAULT = 0, TOP = 1, SIDE = 2, TOP_DOOR = 3, SIDE_DOOR = 4 }

}
