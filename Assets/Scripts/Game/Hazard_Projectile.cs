using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Projectile : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 5.0f;

    public void Start()
    {
        gameObject.tag = "Projectile";
    }

    public void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        var obj = collision.gameObject;

        if (obj.CompareTag("Player"))
        {
            Character tempChar = obj.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (obj.CompareTag("Enemy") || obj.CompareTag("Wall") || obj.CompareTag("DungeonContent")) {
            Destroy(gameObject);
        }
    }
}
