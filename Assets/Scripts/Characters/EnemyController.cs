using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    [SerializeField]
    protected float movSpeed = 2.0f;
    [SerializeField]
    protected float rotSpeed = 3.0f;
    [SerializeField]
    protected float despawnTime = 2.0f;

    protected GameObject player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if(anim == null)
            anim = GetComponentInChildren<Animator>();
    }
    
    // Despawns the enemy after allowing them to be dead for a bit
    public override void HandleTakeDamage(bool isDead)
    {
        if(isDead)
            StartCoroutine(HandleDespawn());
    }

    // Handles despawning the body of the boss
    protected IEnumerator HandleDespawn()
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }
}

// Replaced with animation triggers. Possible use later?
/*
         AnimationClip[] clips = animController.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attack":
                    atkClip = clip;
                    break;
                case "Death":
                    dthClip = clip;
                    break;
                case "Idle":
                    idlClip = clip;
                    break;
                case "Running":
                    runClip = clip;
                    break;
                case "Spawn":
                    spnClip = clip;
                    break;
                default:
                    Debug.Log("Unrecognized animation clip: " + clip.name);
                    break;
            }
        }
 */
