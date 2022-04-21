using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Projectile : MonoBehaviour
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

        if (obj.CompareTag("Enemy"))
        {

            Character tempChar = obj.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(damage);
                StartCoroutine(Cleanup());
            }
        }
        else if (obj.CompareTag("Wall") || obj.CompareTag("DungeonContent"))
        {
            StartCoroutine(Cleanup());
        }
    }

    //I should have just had the Player/Enemy play sound effect on cast, rather than the individual projectiles play sound on awake
    private IEnumerator Cleanup()
    {
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }

        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Destroy(gameObject);
    }
}
