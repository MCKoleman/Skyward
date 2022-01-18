using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterSpeedMods", order = 1)]
[System.Serializable]
public class CharacterSpeedMods : ScriptableObject
{
    [Range(1.0f, 50.0f), Tooltip("Speed at which the character moves")]
    public float MOVE_MOD = 5.0f;
    [Range(1.0f, 50.0f), Tooltip("Speed at which the character speeds up after starting movement")]
    public float MOVE_SPEED_MOD = 5.0f;
    [Range(1.0f, 50.0f), Tooltip("Speed at which the character slows down after stopping movement")]
    public float MOVE_SLOW_MOD = 5.0f;
    [Range(1.0f, 50.0f), Tooltip("Force with which the character jumps")]
    public float JUMP_MOD = 7.0f;
    [Range(1.0f, 50.0f), Tooltip("Force with which the character dashes")]
    public float DASH_MOD = 10.0f;
    [Range(0.0f, 5.0f), Tooltip("Duration of the dash, during which the player is immune to physics")]
    public float DASH_DURATION = 0.2f;
}
