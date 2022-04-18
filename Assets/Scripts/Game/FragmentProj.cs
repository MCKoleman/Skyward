using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentProj : MonoBehaviour
{
    public GameObject fragProj;
    public float timer = 2.0f;
    public int numFragments = 8;
    public float radius = 1.0f;

    //Can't retrieve Force from rigidbody, can only get velocity
    //Refactor: change projectiles from using force to using velocity
    public int speedMultiplier = 2;
    //public float originalForce = 100.0f;

    private bool fragged = false;

    private void Update()
    {
        if (timer < 0 && !fragged)
        {
            fragged = true;
            Splinter(numFragments, transform.position, radius);
        }

        timer -= Time.deltaTime;
    }

    public void Splinter(int num, Vector3 point, float radius)
    {
        float originalVelocity = GetComponent<Rigidbody>().velocity.magnitude;

        for (int i = 0; i < num; i++) {

            var radians = 2 * Mathf.PI / num * i;

            //Get the direction
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);

            //Spawn fragment
            var spawnPos = point + spawnDir * radius;
            spawnPos.y = gameObject.transform.position.y;
            var fragment = Instantiate(fragProj, spawnPos, Quaternion.identity) as GameObject;

            //Apply force in the direction it was spawned
            var dir = (fragment.transform.position - transform.position).normalized;
            //fragment.GetComponent<Rigidbody>().AddForce(dir * (forceMultiplier * originalForce));
            fragment.GetComponent<Rigidbody>().velocity = (dir * (speedMultiplier * originalVelocity));
        }

        CleanUp();
    }

    //Add any VFX or SFX....
    private void CleanUp()
    {
        Destroy(gameObject);
    }
}
