using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Seek : EnemyController
{
    protected float dist;
    protected bool pause = false;
    public float chaseDist = 5.0f;
    public float stopDist = 1.5f;

    protected enum State { IDLE, CHASE, ATTACK, DEATH };
    protected State curr = State.IDLE;

    protected EnemyAttack_Melee mAttack;

    protected override void Start()
    {
        base.Start();
        mAttack = transform.Find("AttackBox").gameObject.GetComponent<EnemyAttack_Melee>();
        //animController.SetFloat("SpeedMultiplier", 2.0f);
    }

    protected void Update()
    {
        Debug.Log(curr);

        player = GameObject.FindWithTag("Player");

        if (curr != State.DEATH) {

            dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= chaseDist && dist > stopDist && curr != State.ATTACK)
            {
                if (curr != State.CHASE)
                {
                    curr = State.CHASE;
                    animController.SetTrigger("Chase");
                }
                SmoothChase();
            }
            else if (dist <= stopDist)
            {
                if (curr != State.ATTACK && !mAttack.IsCooling())
                {
                    curr = State.ATTACK;
                    animController.SetTrigger("Attack");
                }
                else if (curr != State.ATTACK && curr != State.IDLE)
                {
                    curr = State.IDLE;
                    animController.SetTrigger("Idle");
                }
                FacePlayer();
            }
            else if (dist > chaseDist && curr != State.ATTACK)
            {
                if (curr != State.IDLE)
                {
                    curr = State.IDLE;
                    animController.SetTrigger("Idle");
                }
            }
        }
    }

    // Snaps to player location when in chasing distance
    protected void InstantChase()
    {
        transform.LookAt(player.transform);
        if (dist > stopDist)
        {
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * movSpeed * Time.deltaTime;
        }
    }

    // Turns to player location when in chasing distance
    // Rotation determined by difference in Player's and Enemy's positions (y-axis is omitted)
    protected void SmoothChase()
    {
        FacePlayer();
        transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * movSpeed * Time.deltaTime;

        /*
        if (dist > stopDist)
        {
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * movSpeed * Time.deltaTime;
        }
        */
    }

    protected void FacePlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(
                new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)
                ),
            rotSpeed * Time.deltaTime);
    }

    // Called on animation event
    public void Attack()
    {
        mAttack.ToggleAttack();
    }

    public void ExitAttack()
    {
        mAttack.ExitAttack();
        curr = State.IDLE;
    }
}
