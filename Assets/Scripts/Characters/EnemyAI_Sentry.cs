using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Sentry : EnemyController
{
    protected float dist;
    public float range;

    protected EnemyAttack_Range rAttack;

    protected override void Start()
    {
        base.Start();
        rAttack = GetComponent<EnemyAttack_Range>();
    }

    protected void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform);
        if (rAttack != null && dist <= range)
        {
            rAttack.Fire();
        }
    }
}
