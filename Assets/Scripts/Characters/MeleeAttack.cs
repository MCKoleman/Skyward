using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float cooldown = 0.0f;
    public float duration = 0.0f;
    private bool attacking = false;
    private bool canAttack = true;
    public int damage = 1;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;

        attacking = true;
        yield return new WaitForSeconds(duration);
        attacking = false;

        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Character tempChar = collision.gameObject.GetComponent<Character>();
            if (tempChar != null && attacking)
            {
                tempChar.TakeDamage(damage);
            }
        }
    }
}
