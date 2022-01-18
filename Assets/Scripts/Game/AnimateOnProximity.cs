using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnProximity : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip inRangeClip;
    [SerializeField]
    private AudioClip outOfRangeClip;
    private bool isInRange;

    private void Start()
    {
        // If there is no animator, find one
        if(anim == null)
            anim = this.GetComponent<Animator>();
    }

    // Sets the value of the isInRange bool to the given value in the class and animator
    private void SetInRange(bool _isInRange)
    {
        // Set animation info
        isInRange = _isInRange;
        anim.SetBool("IsInRange", _isInRange);

        // Play audio
        if (_isInRange)
            source.clip = inRangeClip;
        else
            source.clip = outOfRangeClip;
        source.Play();
    }

    // Collision enter handle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only check for player collisions
        if(collision.CompareTag("Player"))
        {
            SetInRange(true);
        }
    }

    // Collision exit handle
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Only check for player collisions
        if (collision.CompareTag("Player"))
        {
            SetInRange(false);
        }
    }
}
