using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Checkpoint class. The currently active checkpoint should be managed by
 * the checkpoint manager, while each checkpoint individually keeps track
 * of whether they are active or not.
 */
public class Checkpoint : MonoBehaviour
{
    // Inspector fields
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource source;

    // State data
    private bool isActive = false;

    // Activates or deactivates this checkpoint
    public void SetActive(bool _isActive = true)
    {
        // Set anim info
        isActive = _isActive;

        // Don't activate animator or sound if they don't both exist
        if(anim != null && source != null)
        {
            anim.SetBool("isActive", _isActive);

            // Play audio
            if (_isActive)
                source.Play();
        }
    }

    // Returns whether this is the currently active checkpoint
    public bool IsActive() { return isActive; }

    // Check collisions
    private void OnTriggerEnter(Collider collision)
    {
        // If the player collides with a checkpoint, set the current checkpoint as the one most recently collided with
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            CheckpointManager.Instance.SetCurCheckpoint(this);
        }
    }
}
