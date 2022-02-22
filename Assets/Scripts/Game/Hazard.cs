using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
<<<<<<< HEAD
    private int damage;
=======
    private int damage = 1;
>>>>>>> 0711b6b30d1bfe326d1ebad86795c9cfa33220b2

    // Check if colliding with the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !collision.collider.isTrigger)
        {
            Character tempChar = collision.collider.GetComponent<Character>();
<<<<<<< HEAD
            if(tempChar != null)
=======
            if (tempChar != null)
>>>>>>> 0711b6b30d1bfe326d1ebad86795c9cfa33220b2
            {
                tempChar.TakeDamage(damage);
            }
        }
    }
}
