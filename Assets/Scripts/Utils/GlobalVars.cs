using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : Singleton<GlobalVars>
{
    public enum ElementType { DEFAULT, FIRE, ICE, LIGHTNING };

    public enum Direction { MIDDLE = 0, TOP = 1, BOTTOM = 2, RIGHT = 3, LEFT = 4 }
}
