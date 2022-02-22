using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUIToCam : MonoBehaviour
{

    void Update()
    {
        Camera camera = Camera.main;

        transform.LookAt(transform.position + camera.transform.rotation * Vector3.up, camera.transform.rotation * Vector3.up);
        transform.Rotate(90f, 0f, 0f);
    }
}
