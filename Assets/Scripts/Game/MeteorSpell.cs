using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : MonoBehaviour
{

    HashSet<Character> inArea;

    public int damage;
    public ParticleSystem vfx;

    private void Start()
    {
        StartCoroutine(Cleanup());
    }

    IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(vfx.main.duration);
        Destroy(this.gameObject);
    }

    public void ImpactDamage()
    {
        foreach(Character enemy in inArea)
        {
            enemy.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Character tempChar = collision.gameObject.GetComponent<Character>();
        if (tempChar != null && collision.gameObject.CompareTag("Enemy"))
        {
            inArea.Add(tempChar);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Character tempChar = collision.gameObject.GetComponent<Character>();
        if (tempChar != null && collision.gameObject.CompareTag("Enemy"))
        {
            inArea.Remove(tempChar);
        }
    }
}
