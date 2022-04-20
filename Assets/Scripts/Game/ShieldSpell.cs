using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : MonoBehaviour
{
    public float duration = 5.0f;

    //Protect parent from any harm
    private void Start()
    {
        StartCoroutine(LifeTime());
    }
    
    IEnumerator LifeTime()
    {
        GetComponentInParent<Character>().ToggleInvincibility(true);
        yield return new WaitForSeconds(duration);
        GetComponentInParent<Character>().ToggleInvincibility(false);
        Destroy(this.gameObject);
    }
}
