using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Miniboss : EnemyAI_Seek
{
    // Sets the AIState and animation trigger based on the new state
    protected override void SetAIState(AIState _state)
    {
        curState = _state;
        anim.SetBool("Run Forward", curState == AIState.CHASE);
        switch (curState)
        {
            case AIState.ATTACK:
                // Randomly choose between stab and smash attack
                if (Random.Range(0, 3) != 0)
                    anim.SetTrigger("Stab Attack");
                else
                    anim.SetTrigger("Smash Attack");
                break;
            case AIState.DEATH:
                anim.SetTrigger("Die");
                break;
            default:
                break;
        }
    }

    // Handles reacting to taking damage
    public override void HandleTakeDamage(bool isDead)
    {
        base.HandleTakeDamage(isDead);
        if (isDead)
        {
            SpawnExit();
        }
        else
        {
            anim.SetTrigger("Take Damage");
        }
    }

    // Spawns the exit when the boss dies
    protected void SpawnExit()
    {
        Instantiate(PrefabManager.Instance.visagePrefab, new Vector3(0, 0, -2), Quaternion.identity, PrefabManager.Instance.visageHolder);
        GameObject exit = Instantiate(PrefabManager.Instance.exitPrefab, Vector3.zero, Quaternion.identity, PrefabManager.Instance.levelHolder);
        exit.GetComponentInChildren<DialogueTrigger>().isEnabled = true;
    }
}
