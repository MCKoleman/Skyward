using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    // Cast cooldown (should be < spell cooldowns)
    public float castCooldown;
    private float castTimer;

    // Spells to instantiate
    [Tooltip("Meteor - 0, Frost - 1, Lightning - 2, Magic Missile - 3, Shield - 4")]
    public GameObject[] spells = new GameObject[5];

    // Cooldowns for spells
    [Tooltip("Meteor - 0, Frost - 1, Lightning - 2, Magic Missile - 3, Shield - 4")]
    public float[] cooldowns = new float[5];

    // Cooldown indicators
    private bool[] checks = new bool[5];

    public float projSpeed;
    public GameObject projSpawn;
    private delegate void UpdateUI(float percent);
    private Ray viewRay;

    //Update loop wouldn't work when invoking UpdateSpellCooldown for some reason
    IEnumerator CastCooldown()
    {
        castTimer = castCooldown;

        //Cast cooldown
        while (castTimer > 0)
        {
            UIManager.Instance.UpdateSpellCooldown(castTimer / castCooldown);
            castTimer -= Time.deltaTime;
            yield return null;
        }
        UIManager.Instance.UpdateSpellCooldown(0.0f);
    }

    //Rather than having separate cooldowns for each spell
    //Also allows having unique spells cooldowns, within the same casting cooldown, at the same time
    IEnumerator Cooldown(int spell, UpdateUI updateFunc)
    {
        StartCoroutine(CastCooldown());

        checks[spell] = true;
        var timeLeft = cooldowns[spell];
        while (timeLeft > 0)
        {
            updateFunc(timeLeft/cooldowns[spell]);
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        updateFunc(0.0f);
        checks[spell] = false;
    }

    public void Meteor()
    {
        if (castTimer <= 0 && !checks[0] && CastAtMouse(spells[0]))
        {
            Debug.Log("AAAAAAAAAAUGH!!!");
            StartCoroutine(Cooldown(0, UIManager.Instance.UpdateAbility1Cooldown));
        }
    }

    public void Frost()
    {
        if (castTimer <= 0 && !checks[1] && CastAtPos(spells[1]))
        {
            Debug.Log("CHILLY!!!");
            StartCoroutine(Cooldown(1, UIManager.Instance.UpdateAbility2Cooldown));
        }
    }

    public void Lightning()
    {
        if (castTimer <= 0 && !checks[2] && CastAtMouse(spells[2]))
        {
            Debug.Log("THUNDER!!!");
            StartCoroutine(Cooldown(2, UIManager.Instance.UpdateAbility3Cooldown));
        }
    }

    public void MagicMissile() {
        if (castTimer <= 0 && !checks[3] && CastAtDirection(spells[3]))
        {
            Debug.Log("MAGIC MISSILE!!!");
            StartCoroutine(Cooldown(3, UIManager.Instance.UpdateAbility0Cooldown));
        }
    }
	
    public void Shield()
    {
        if (castTimer <= 0 && !checks[4] && CastInParent(spells[4]))
        {
            Debug.Log("SHIELDS UP!!!");
            StartCoroutine(Cooldown(4, UIManager.Instance.UpdateShieldCooldown));
        }
    }

    //Instantiate the spell. Spell-specific functionality is implemented in their respective scripts.
    //Return bool. Only want spell to cooldown when it has actually triggered.
    private bool CastAtMouse(GameObject spell)
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(viewRay, out hit, 100.0f, mask))
        {
            Debug.Log(hit.transform.name);
            Instantiate(spell, hit.point, Quaternion.identity);
            return true;
        }

        return false;
    }

    private bool CastAtPos(GameObject spell)
    {
        Instantiate(spell, transform.position, transform.rotation);
        return true;
    }

    private bool CastInParent(GameObject spell)
    {
        var magic = Instantiate(spell, transform.position, transform.rotation) as GameObject;
        magic.transform.SetParent(transform);
        return true;
    }

    private bool CastAtDirection(GameObject spell)
    {
        var magic = Instantiate(spell, projSpawn.transform.position, projSpawn.transform.rotation, PrefabManager.Instance.projectileHolder) as GameObject;
        magic.GetComponent<Rigidbody>().AddForce(magic.transform.forward * projSpeed);
        //GetComponent<AudioSource>().PlayOneShot(magic.g);
        return true;
    }

    public void SetRay(Ray fromMouse)
    {
        viewRay = fromMouse;
    }
}
