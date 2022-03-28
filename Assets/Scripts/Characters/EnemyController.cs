using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    [SerializeField]
    protected float movSpeed = 2.0f;
    [SerializeField]
    protected float rotSpeed = 3.0f;

    protected GameObject player;

    protected Animator animController;
    protected AnimationClip atkClip;
    protected AnimationClip dthClip;
    protected AnimationClip idlClip;
    protected AnimationClip runClip;
    protected AnimationClip spnClip;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");

        animController = GetComponent<Animator>();
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
    }
}
