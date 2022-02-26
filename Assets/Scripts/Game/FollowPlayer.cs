using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private GameObject player;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        this.transform.position = player.transform.position + offset;
    }
}
