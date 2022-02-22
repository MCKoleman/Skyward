using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{

    private GameObject player;
    private float dist;
    public float chaseDist = 5.0f;
    public float stopDist = 1.5f;
    public float speed = 2.0f;
    public float rotSpeed = 3.0f;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
    }

    protected void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= chaseDist)
        {
            SmoothChase();
        }
    }

    // Snaps to player location when in chasing distance
    protected void InstantChase()
    {
        transform.LookAt(player.transform);
        if (dist > stopDist)
        {
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * speed * Time.deltaTime;
        }
    }

    // Turns to player location when in chasing distance
    protected void SmoothChase()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(player.transform.position - transform.position),
            rotSpeed * Time.deltaTime);

        if (dist > stopDist)
        {
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * speed * Time.deltaTime;
        }
    }

}
