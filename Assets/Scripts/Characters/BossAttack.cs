using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // Attack 1: contact with player
    public int contactDmg;
    public float contactCooldown;
    private bool canHurt = true;

    // Attack 2: projectiles
    [System.Serializable]
    public class Projectile
    {
        public Transform projectile;
        public float chance;
        public float speed;
        public float timeBetween;
        public int minVolley;
        public int maxVolley;
    }

    public Transform projPoint;
    public Projectile[] projectiles;
    private Projectile currMissile;

    // Attack 3: orbiting barriers/projectiles
    [System.Serializable]
    public class Barrier
    {
        public Transform projectile;
        public int min;
        public int max;
        public float distance;
        public float rotSpeed;
        public float disperseForce;
        public float timeBetween;
        [HideInInspector]
        public List<GameObject> curr;
    }

    public Barrier barriers;
    private bool barriersActive;

    // Misc. VFX
    public ParticleSystem teleVFX;
    public float teleTime;
    public ParticleSystem deathVFX;
    public float deathTime;

    private void LateUpdate()
    {
        //Movement determined in Update, want barriers to rotate after new movement position has been determiend
        ManageBarriers();
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(contactCooldown);
        canHurt = true;
    }

    // On collision with Player during ATTACK state, deal damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && barriersActive && canHurt)
        {
            //barriersActive is only true when ATTACK state is in progress
            canHurt = false;
            collision.gameObject.GetComponent<PlayerCharacter>().TakeDamage(contactDmg);
            StartCoroutine(Cooldown());
            Debug.Log("CONTACT");
        }
    }

    public IEnumerator MagicMissiles()
    {
        // Randomly choose between the projectile options
        // Note: chance of selection is based on chance variable AND position in list
        // Ex: 0.7f chance for proj1 and 1.0f chance for proj2 is 70/30 chance respectively
        Random.InitState(System.DateTime.Now.Millisecond);
        foreach (Projectile projectile in projectiles)
        {
            if (Random.value > (1 - projectile.chance))
            {
                currMissile = projectile;
                break;
            }
        }

        // Spawn projectiles
        int volleySize = Mathf.CeilToInt(Random.Range(currMissile.minVolley, currMissile.maxVolley));
        for (int i = 0; i < volleySize; i++)
        {
            var temp = Instantiate(currMissile.projectile, projPoint.position, projPoint.rotation);
            temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * currMissile.speed);
            yield return new WaitForSeconds(currMissile.timeBetween);
        }
    }

    public void ReleaseBarriers()
    {
        //Stop the rotation
        barriersActive = false;

        List<GameObject> temp = new List<GameObject>(barriers.curr);
        foreach (GameObject barrier in temp)
        {
            //Turn into independent projectile
            barrier.transform.SetParent(null);

            //Apply force in the direction it was spawned
            var dir = (barrier.transform.position - transform.position).normalized;
            barrier.GetComponent<Rigidbody>().velocity = (dir * barriers.disperseForce);
            StartCoroutine(barrier.GetComponent<Hazard_Trigger>().CleanUp());
            barriers.curr.Remove(barrier);
        }

        //Assuming the projectile will come in contact with a wall and destroy itself...
        barriers.curr.TrimExcess();
    }

    //Done in LateUpdate now
    private void ManageBarriers()
    {
        //Note: this depends on barriers being parented by boss
        if (barriersActive)
        {
            //Rotate each of the barriers around the pivot
            foreach (GameObject barrier in barriers.curr)
            {
                barrier.transform.RotateAround(transform.position, transform.up, barriers.rotSpeed * Time.deltaTime);
            }
        }
    }

    //For reference
    /*
    private IEnumerator ManageBarriers()
    {
        while (barriersActive)
        {
            //Rotate each of the barriers around the pivot
            foreach (GameObject barrier in barriers.curr)
            {
                barrier.transform.RotateAround(transform.position, transform.up, barriers.rotSpeed * Time.deltaTime);
            }

            yield return null;  //prevent infinite loop
        }

        yield break;
    }
    */

    public IEnumerator MagicBarriers()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int num = Mathf.CeilToInt(Random.Range(barriers.min, barriers.max));

        for (int i = 0; i < num; i++)
        {

            var radians = 2 * Mathf.PI / num * i;

            //Get the direction
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);

            //Spawn
            var spawnPos = transform.position + spawnDir * barriers.distance;
            spawnPos.y = projPoint.transform.position.y;
            var fragment = Instantiate(barriers.projectile.gameObject, spawnPos, Quaternion.identity) as GameObject;
            fragment.transform.SetParent(transform);

            //Keep track of spawned
            barriers.curr.Add(fragment);

            yield return new WaitForSeconds(barriers.timeBetween);
        }

        barriersActive = true;
        //StartCoroutine(ManageBarriers());
        yield break;
    }

    public float Teleport()
    {
        Instantiate(teleVFX, transform.position, Quaternion.identity);
        return teleVFX.GetComponent<ParticleSystem>().main.duration;

        //note about teleVFX:
        //currently sim speed 1.5 w/ dur 0.6 works, but it's hardcoded
        //test: simulation speed 3 and dur 0.8, disappear object after duration, use callback when system finishes to trigger teleport
        //that way vfx is part of boss and not instantiated
    }
}
