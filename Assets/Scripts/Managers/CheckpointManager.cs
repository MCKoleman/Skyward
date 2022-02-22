using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : Singleton<CheckpointManager>
{
    // Inspector fields
    [SerializeField]
    private Checkpoint curCheckpoint;

    // State info
    private PlayerController player;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
    }

    // Resets the player to the checkpoint when they die
    public void ResetToCheckpoint()
    {
        // Only reset the player if they exist
        if (GetPlayer() != null)
        {
            // If a checkpoint does not exist, move the player to the position zero which should always be within game bounds
            player.RespawnPlayer();
            player.transform.position = (curCheckpoint != null) ? curCheckpoint.transform.position : Vector3.zero;
            Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
        }
    }

    // Returns a reference to the player controller
    private PlayerController GetPlayer()
    {
        // If the player controller reference is null, try to find a player
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();

        // Return the player even if it is null
        return player;
    }

    // Getters and setters
    public Checkpoint GetCurCheckpoint() { return curCheckpoint; }

    public void SetCurCheckpoint(Checkpoint _checkpoint)
    {
        // Don't swap checkpoints if colliding with the current checkpoint
        if(_checkpoint != curCheckpoint)
        {
            // Swap the currently active checkpoint states
            curCheckpoint?.SetActive(false);
            _checkpoint?.SetActive(true);

            // Set current checkpoint
            curCheckpoint = _checkpoint;
        }
    }
}
