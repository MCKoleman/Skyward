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
        // Disable rotation when dead
        if (isDead)
            return;

        dist = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        if (rAttack != null && dist <= range)
        {
            if (rAttack.AttackState())
            {
                rAttack.ToggleAttack(false);
                anim.SetTrigger("Attack");
            }
        }
    }

    public override void HandleTakeDamage(bool isDead)
    {
        base.HandleTakeDamage(isDead);
        if (!isDead)
            anim.SetTrigger("Hurt");
        else
            anim.SetTrigger("Death");
    }
}
