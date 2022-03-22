using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Melee : MonoBehaviour
{
    public int damage = 1;
    public float delay = 0.5f;
    public float cooldown = 2.0f;
    private bool delaying = true;
    private bool attacking = false;

    //TEMPORARY. THIS IS NOT PRETTY AND BEING REWORKED.

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        delaying = false;
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(cooldown);
        attacking = false;
        delaying = true;
        //gameObject.transform.parent.GetComponent<EnemyAI_Seek>().ToggleChase();
    }

    private void OnTriggerEnter(Collider other)
    {
        delaying = true;
        attacking = false;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character tempChar = collision.gameObject.GetComponent<Character>();
            if (tempChar != null && delaying)
            {
                StartCoroutine(Delay());
            }
            if (tempChar != null && !delaying && !attacking)
            {
                attacking = true;
                //gameObject.transform.parent.GetComponent<EnemyAI_Seek>().ToggleChase();
                tempChar.TakeDamage(damage);
                StartCoroutine(CoolDown());
            }
        }
    }
}
