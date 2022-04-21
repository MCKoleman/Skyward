using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostSpell : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider collision)
    {
        Character tempChar = collision.gameObject.GetComponent<Character>();
        if (tempChar != null && collision.gameObject.CompareTag("Enemy"))
        {
            tempChar.TakeDamage(damage);
        }
    }
}
