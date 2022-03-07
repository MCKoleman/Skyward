using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueList", menuName = "ScriptableObjects/DialogueList", order = 1)]
[System.Serializable]
public class DialogueList : ScriptableObject
{
    [SerializeField]
    private List<DialogueItem> dialogue = new List<DialogueItem>();

    // Returns the dialogue item at the given index
    public DialogueItem GetDialogueAtIndex(int index)
    {
        return dialogue[index];
    }

    // Size of the dialogue list
    public int Count { get { return dialogue.Count; } }
}
