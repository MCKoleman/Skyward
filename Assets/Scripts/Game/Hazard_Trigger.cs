using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Trigger : MonoBehaviour
{
    public int damage = 1;
    public float cooldown = 1.0f;
    public float destructTime = 2.0f;
    private bool canHurt = true;

    public IEnumerator CleanUp()
    {
        yield return new WaitForSeconds(destructTime);
        Destroy(gameObject);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canHurt = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Character tempChar = collision.GetComponent<Character>();
        if (tempChar != null && canHurt)
        {
            tempChar.TakeDamage(damage);
            canHurt = false;
            StartCoroutine(Cooldown());
        }
    }
}
