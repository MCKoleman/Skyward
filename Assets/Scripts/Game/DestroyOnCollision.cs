using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    //private Character character;
    //private bool isPlayer;

    void Start()
    {
        //character = this.GetComponent<Character>();
        //isPlayer = (character is PlayerCharacter);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't collide with friendly targets
        if((collision.collider.CompareTag("Player")) || (collision.collider.CompareTag("Enemy")))
        {
            /*
            // If collided, handle collision on both player and enemy
            Character collidedChar = collision.collider.GetComponent<Character>();
            if (collidedChar != null)
            {
                collidedChar.OnEnemyCollision(character);
                character.OnEnemyCollision(collidedChar);
            }
            */
        }
    }
}
