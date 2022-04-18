using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Sentry_NoAnim : EnemyController
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
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        if (rAttack != null && dist <= range)
        {
            if (rAttack.AttackState())
            {
                rAttack.ToggleAttack(false);
                StartCoroutine(rAttack.ShootAndRest());
            }
        }
    }
}
