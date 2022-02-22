using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !collision.collider.isTrigger)
        {
            Character tempChar = collision.collider.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(damage);
            }
        }
    }
}
