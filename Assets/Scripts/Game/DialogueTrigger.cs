using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If colliding with the player, enable the next dialogue option
        if(other.CompareTag("Player"))
        {
            DialogueManager.Instance.BeginDialogue();
        }
    }
}
