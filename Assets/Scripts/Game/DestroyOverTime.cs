using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField]
    private float lifetime;

    void Start()
    {
        StartCoroutine(DestroyAfterTime(lifetime));
    }

    // Destroys the game object after given time
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
