using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Projectile : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Character tempChar = collision.gameObject.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
