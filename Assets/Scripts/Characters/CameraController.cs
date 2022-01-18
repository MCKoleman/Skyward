using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    protected Transform cameraPivot;
    [SerializeField]
    protected float rotateMod = 0.1f;
    [SerializeField]
    protected bool invertX = false;
    [SerializeField]
    protected bool invertY = true;

    public void HandleLook(Vector2 delta)
    {
        Vector2 tempInvert = new Vector2(invertX ? -1.0f : 1.0f, invertY ? -1.0f : 1.0f);
        cameraPivot.Rotate(new Vector3(delta.y * tempInvert.x, delta.x * tempInvert.y, 0.0f) * rotateMod);
    }


}
