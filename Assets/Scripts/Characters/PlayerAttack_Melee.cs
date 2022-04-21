using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttack_Melee : MonoBehaviour
{
    public float cooldown = 1.0f;
    public float duration = 0.5f;
    private bool canAttack = true;
    public int damage = 1;

    public VisualEffect slash;

    private void Start()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public bool Attack()
    {
        if (canAttack)
        {
            StartCoroutine(HandleAttack());
            return true;
        }

        return false;
    }

    //Temporarily turn on trigger and play VFX
    IEnumerator HandleAttack()
    {
        canAttack = false;
        GetComponent<CapsuleCollider>().enabled = true;
        slash.Play();
        Camera.main.GetComponent<CameraController>().Shake(0.1f, 0.1f);
        yield return new WaitForSeconds(duration);
        GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    //If enemy is caught in trigger when it's turned on, they take damage
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Character tempChar = collision.gameObject.GetComponent<Character>();
            if (tempChar != null)
            {
                tempChar.TakeDamage(damage);
            }
        }
    }
}