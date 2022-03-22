using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    // Use trigger collision
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Character tempChar = collision.GetComponent<Character>();
        if (tempChar != null)
        {
            tempChar.TakeDamage(damage);
        }
    }

    // Check if colliding with the player
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player") || collision.collider.isTrigger)
            return;

        Character tempChar = collision.collider.GetComponent<Character>();
        if (tempChar != null)
        {
            tempChar.TakeDamage(damage);
        }
    }
}
