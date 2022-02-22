using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Check if colliding with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with a hazard, reset them to the last checkpoint
        if(collision.collider.CompareTag("Player"))
        {
            Debug.Log("Killed player");
            CheckpointManager.Instance.ResetToCheckpoint();
        }
    }
}
