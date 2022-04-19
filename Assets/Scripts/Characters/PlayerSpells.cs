using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    // Spells to instantiate
    [Tooltip("Lightning - 0, Forst - 1, Meteor - 2, Shield - 3")]
    public GameObject[] spells = new GameObject[4];

    // Cooldowns for spells
    public float[] cooldowns = new float[4];

    // Cooldown indicators
    private bool[] checks = new bool[4];

    private Ray viewRay;

    //Sure wish I could pass references
    IEnumerator Cooldown(int spell)
    {
        checks[spell] = false;
        yield return new WaitForSeconds(cooldowns[spell]);
        checks[spell] = true;
    }

    public void Lightning()
    {
        if (checks[0])
        {
            CastAtMouse(spells[0]);
            StartCoroutine(Cooldown(0));
        }
    }

    public void Frost()
    {
        if (checks[1])
        {
            //CastAtPos(spells[1]);
            StartCoroutine(Cooldown(1));
        }
    }

    public void Meteor()
    {
        if (checks[2])
        {
            CastAtMouse(spells[2]);
            StartCoroutine(Cooldown(2));
        }
    }

    public void Shield()
    {
        if (checks[3])
        {
            //Don't cast, just make the palyer invincible
            StartCoroutine(Cooldown(3));
        }
    }

    //Instantiate the spell. Spell-specific functionality is implemented in their respective scripts.
    private void CastAtMouse(GameObject spell)
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(viewRay, out hit, mask))
        {
            Instantiate(spell, hit.point, Quaternion.identity);
        }
    }

    private void CastAtPos()
    {
        //Instantiate(spell, transform.position, transform.rotation);
    }

    public void SetRay(Ray fromMouse)
    {
        viewRay = fromMouse;
    }
}
