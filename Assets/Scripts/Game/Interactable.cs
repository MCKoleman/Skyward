using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [System.Serializable]
    public enum InteractableType { NONE, DOOR}

    public InteractableType type;
    public int value;
}
