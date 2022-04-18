using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbilities : MonoBehaviour
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
    private bool gameOver = false;

    // Attack 4: spawn baddies
    [System.Serializable]
    public class Henchmen
    {
        public Transform enemy;
        public int min;
        public int max;
        public float timeBetween;
        [HideInInspector]
        public List<GameObject> curr;
    }

    public Henchmen backup;

    // Misc. VFX
    public GameObject teleVFX;
    public GameObject spawnVFX;
    public GameObject deathVFX;

    private void LateUpdate()
    {
        //Movement determined in Update, want barriers to rotate after new movement position has been determiend
        ManageBarriers();
    }

    /*===============================ATTACK 1: CONTACT===============================*/
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

    /*===============================ATTACK 2: PROJECTILES===============================*/
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
        for (int i = 0; !gameOver && i < volleySize; i++)
        {
            var temp = Instantiate(currMissile.projectile, projPoint.position, projPoint.rotation);
            temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * currMissile.speed);
            yield return new WaitForSeconds(currMissile.timeBetween);
        }
    }

    /*===============================ATTACK 3: BARRIERS===============================*/
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
        if (!gameOver && barriersActive)
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

        for (int i = 0; !gameOver && i < num; i++)
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

    /*===============================ATTACK 4: ENEMY BACKUP===============================*/
    public IEnumerator GoodbyeHenchman(GameObject baddie)
    {
        Instantiate(teleVFX, baddie.transform.position, Quaternion.identity);
        yield return teleVFX.GetComponent<ParticleSystem>().main.duration;
        Destroy(baddie);
        yield break;
    }

    public IEnumerator GoAwayBaddies()
    {
        List<GameObject> temp = new List<GameObject>(backup.curr);
        foreach (GameObject baddie in temp)
        {
            if (baddie != null)
            {
                StartCoroutine(GoodbyeHenchman(baddie));
            }

            backup.curr.Remove(baddie);
            yield return new WaitForSeconds(backup.timeBetween);
        }

        backup.curr.TrimExcess();
        yield break;
    }

    public IEnumerator Spawn(Vector3 spawnPos, GameObject obj)
    {
        Instantiate(spawnVFX, spawnPos, Quaternion.identity);
        yield return spawnVFX.GetComponent<ParticleSystem>().main.duration;
        var baddie = Instantiate(obj, spawnPos, Quaternion.identity) as GameObject;

        //Keep track of spawned
        backup.curr.Add(baddie);
    }

    public IEnumerator SpawnBaddies(float maxX, float minX, float minZ, float maxZ)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int num = Mathf.CeilToInt(Random.Range(backup.min, backup.max));

        for (int i = 0; !gameOver && i < num; i++)
        {

            Random.InitState(System.DateTime.Now.Millisecond);

            //Spawn
            var spawnPos = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
            StartCoroutine(Spawn(spawnPos, backup.enemy.gameObject));

            yield return new WaitForSeconds(backup.timeBetween);
        }

        yield break;
    }

    /*===============================ADITIONAL ABILITIES / MISC.===============================*/
    public float Teleport()
    {
        Instantiate(teleVFX, transform.position, Quaternion.identity);
        return teleVFX.GetComponent<ParticleSystem>().main.duration;
    }

    //Refactor these to reuse the particle system instead of instantiating objects
    //I couldn't figure out a good way to synchronize the AI with the VFXs in time, so hopefully this works in the actual build
    public float SelfDestruct()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
        return deathVFX.GetComponent<ParticleSystem>().main.duration;
    }

    public void DisableAbilities()
    {
        gameOver = true;
        ReleaseBarriers();
    }
}
