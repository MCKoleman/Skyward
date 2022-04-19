using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Seek : EnemyController
{
    protected float dist;
    protected bool pause = false;
    public float chaseDist = 5.0f;
    public float stopDist = 1.5f;

    protected enum AIState { IDLE, CHASE, ATTACK, DEATH };
    protected AIState curState = AIState.IDLE;

    protected EnemyAttack_Melee mAttack;

    protected override void Start()
    {
        base.Start();
        mAttack = transform.Find("AttackBox").gameObject.GetComponent<EnemyAttack_Melee>();
        //animController.SetFloat("SpeedMultiplier", 2.0f);
    }

    protected void Update()
    {
        // Only search for player if it has not been found yet
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        // Don't update AI when dead
        if (curState == AIState.DEATH)
            return;

        // Evaluate AI conditions
        dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
        if (dist <= chaseDist && dist > stopDist && curState != AIState.ATTACK)
        {
            if (curState != AIState.CHASE)
            {
                SetAIState(AIState.CHASE);
            }
            SmoothChase();
        }
        else if (dist <= stopDist)
        {
            if (curState != AIState.ATTACK && !mAttack.IsCooling())
            {
                SetAIState(AIState.ATTACK);
            }
            else if (curState != AIState.ATTACK && curState != AIState.IDLE)
            {
                SetAIState(AIState.IDLE);
            }
            FacePlayer();
        }
        else if (dist > chaseDist && curState != AIState.ATTACK)
        {
            if (curState != AIState.IDLE)
            {
                SetAIState(AIState.IDLE);
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

    // Sets the AIState and animation trigger based on the new state
    protected virtual void SetAIState(AIState _state)
    {
        curState = _state;
        switch(curState)
        {
            case AIState.ATTACK:
                anim.SetTrigger("Attack");
                break;
            case AIState.IDLE:
                anim.SetTrigger("Idle");
                break;
            case AIState.CHASE:
                anim.SetTrigger("Chase");
                break;
            case AIState.DEATH:
                anim.SetTrigger("Death");
                break;
            default:
                break;
        }
    }

    // Handles killing the character
    public override void HandleTakeDamage(bool isDead)
    {
        base.HandleTakeDamage(isDead);
        if (isDead)
        {
            SetAIState(AIState.DEATH);
        }
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

    // Called on animation event
    public void ExitAttack()
    {
        mAttack.ExitAttack();

        // Cancel switching to idle state when dead
        if(curState != AIState.DEATH)
            curState = AIState.IDLE;
    }
}
