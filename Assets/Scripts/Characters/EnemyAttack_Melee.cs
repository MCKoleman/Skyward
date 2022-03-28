using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Melee : MonoBehaviour
{
    public int damage = 1;
    public float cooldown = 2.0f;
    private bool cooling = false;
    private bool canAttack = false;

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(cooldown);
        cooling = false;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character tempChar = collision.gameObject.GetComponent<Character>();
            if (tempChar != null && canAttack && !cooling)
            {
                canAttack = false;
                cooling = true;
                tempChar.TakeDamage(damage);
            }
        }
    }

    public void ToggleAttack() 
    {
        canAttack = true;
    }

    public void ExitAttack()
    {
        canAttack = false;
        cooling = true;
        StartCoroutine(CoolDown());
    }

    public bool IsCooling()
    {
        return cooling;
    }
}
