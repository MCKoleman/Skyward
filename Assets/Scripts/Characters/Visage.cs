using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visage : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        RotateToPlayer();
    }

    private void RotateToPlayer()
    {
        this.transform.LookAt(player.transform);
    }
}
