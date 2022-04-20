using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool isEnabled = true;

    private void OnTriggerEnter(Collider other)
    {
        // If colliding with the player, enable the next dialogue option
        if(other.CompareTag("Player") && isEnabled)
        {
            DialogueManager.Instance.BeginDialogue();
            this.gameObject.SetActive(false);
        }
    }
}
