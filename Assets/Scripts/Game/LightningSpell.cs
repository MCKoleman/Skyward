using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : MonoBehaviour
{
    public int damage;
    public ParticleSystem vfx;

    private void Start()
    {
        StartCoroutine(Cleanup());
    }

    IEnumerator Cleanup()
    {
        //Debug.Log(vfx.time);
        yield return new WaitForSeconds(vfx.main.duration);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Character tempChar = collision.gameObject.GetComponent<Character>();
        if (tempChar != null && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            tempChar.TakeDamage(damage);
        }
    }
}
