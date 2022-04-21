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
        inArea = new HashSet<Character>();
        StartCoroutine(Cleanup());
    }

    IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(vfx.main.duration);
        Destroy(this.gameObject);
    }

    public void ImpactDamage()
    {
        Debug.Log("impact");
        foreach(Character enemy in inArea)
        {
            Debug.Log("Hit");
            enemy.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Character tempChar = collision.gameObject.GetComponent<Character>();
        if (tempChar != null && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Added");
            inArea.Add(tempChar);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Character tempChar = collision.gameObject.GetComponent<Character>();
        if (tempChar != null && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Removed");
            inArea.Remove(tempChar);
        }
    }
}
