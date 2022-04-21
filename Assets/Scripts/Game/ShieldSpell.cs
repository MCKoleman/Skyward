using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : MonoBehaviour
{
    public float duration = 5.0f;
    public float yOffset;
    public AudioClip deactivateSFX;

    //Protect parent from any harm
    private void Start()
    {
        transform.localPosition = Vector3.zero;
        StartCoroutine(LifeTime());
    }
    
    IEnumerator LifeTime()
    {
        GetComponentInParent<Character>().ToggleInvincibility(true);
        yield return new WaitForSeconds(duration);
        GetComponentInParent<Character>().ToggleInvincibility(false);

        // Deactivate sound
        if (deactivateSFX != null) {
            GetComponent<ParticleSystem>().Stop();
            GetComponent<AudioSource>().PlayOneShot(deactivateSFX);
            yield return new WaitForSeconds(deactivateSFX.length);
        }

        Destroy(this.gameObject);
    }
}
