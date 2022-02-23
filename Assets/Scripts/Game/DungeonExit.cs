using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    // Exits the dungeon, completing it
    private void ExitDungeon()
    {
        Print.Log("Level won!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            ExitDungeon();
        }
    }
}
