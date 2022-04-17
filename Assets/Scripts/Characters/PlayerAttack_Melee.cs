using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Melee : MonoBehaviour
{
    public float cooldown = 1.0f;
    public float duration = 0.5f;
    private bool canAttack = true;
    public int damage = 1;

    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    public void Attack()
    {
        if (canAttack)
            StartCoroutine(HandleAttack());
    }

    IEnumerator HandleAttack()
    {
        canAttack = false;
        GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(duration);
        GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Character tempChar = collision.gameObject.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(damage);
            }
        }
    }
}