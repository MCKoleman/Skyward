using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Melee : MonoBehaviour
{
    public int damage = 1;
    public float cooldown = 0.0f;
    private bool attacking = false;

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(cooldown);
        attacking = false;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character tempChar = collision.gameObject.GetComponent<Character>();
            if (tempChar != null && !attacking)
            {
                attacking = true;
                tempChar.TakeDamage(damage);
                StartCoroutine(CoolDown());
            }
        }
    }
}
