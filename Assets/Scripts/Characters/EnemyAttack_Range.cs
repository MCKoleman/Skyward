using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Range : MonoBehaviour
{
    public float cooldownTime = 5.0f;
    private bool canAttack = true;

    public int volleySize = 1;
    public float projSpeed = 100.0f;
    //public float velocitySpeed = 5.0f;
    public float timeBetweenProj = 0.5f;

    public Transform spawnPoint;
    public Transform projectile;

    public IEnumerator ShootAndRest()
    {
        for (int i = 0; i < volleySize; i++)
        {
            var temp = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * projSpeed);
            //temp.GetComponent<Rigidbody>().velocity = temp.transform.forward * projSpeed;
            yield return new WaitForSeconds(timeBetweenProj);
        }

        yield return new WaitForSeconds(cooldownTime);
        canAttack = true;
    }

    // Animation event
    public void Attack()
    {
        StartCoroutine(ShootAndRest());
    }

    public bool AttackState()
    {
        return canAttack;
    }

    public void ToggleAttack(bool set)
    {
        canAttack = set;
    }
}
