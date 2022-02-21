using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    private int damage;

    // Check if colliding with the player
    private void OnCollisionEnter(Collision collision)
    {
        // If the player collides with a hazard, reset them to the last checkpoint
        if(collision.collider.CompareTag("Player"))
        {
            Character tempChar = collision.collider.GetComponent<Character>();
            if(tempChar != null)
            {
                tempChar.TakeDamage(damage);
            }
        }
    }
}
