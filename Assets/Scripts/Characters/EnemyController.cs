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

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
    }
}
