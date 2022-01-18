using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private Vector3 posOffset;
    private Vector3 tempPos;

    [SerializeField]
    private float amplitude = 0.5f;
    [SerializeField]
    private float frequency = 1.0f;

    void Start()
    {
        posOffset = this.transform.position;
    }

    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        this.transform.position = tempPos;
    }
}
