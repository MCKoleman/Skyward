using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    protected Transform cameraPivot;
    [SerializeField]
    protected CameraShake shake;
    [SerializeField]
    protected float rotateMod = 0.1f;
    [SerializeField]
    protected bool invertX = false;
    [SerializeField]
    protected bool invertY = true;

    private void Start()
    {
        shake = Camera.main.GetComponentInParent<CameraShake>();
    }

    public void HandleLook(Vector2 delta)
    {
        Vector2 tempInvert = new Vector2(invertX ? -1.0f : 1.0f, invertY ? -1.0f : 1.0f);
        cameraPivot.Rotate(new Vector3(delta.y * tempInvert.x, delta.x * tempInvert.y, 0.0f) * rotateMod);
    }

    // Shakes the camera
    public void Shake(float duration = 0.5f, float magnitude = 0.7f, float damping = 1.0f)
    {
        shake.TriggerShake(duration, magnitude, damping);
    }
}
