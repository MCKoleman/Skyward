using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeDuration = 0.0f;
    [SerializeField]
    private float baseShakeMagnitude = 0.7f;
    [SerializeField]
    private float baseDampingSpeed = 1.0f;

    private float shakeMagnitude;
    private float dampingSpeed;
    private Vector3 initialPosition;

    private void OnEnable()
    {
        initialPosition = this.transform.localPosition;
        dampingSpeed = baseDampingSpeed;
        shakeMagnitude = baseShakeMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        // Only run shake when time is enabled
        if(Time.timeScale != 0.0f)
        {
            if (shakeDuration > 0)
            {
                transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude; // * VFXManager.Instance.GetShakeIntensity(shakeMagnitude);
                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0.0f;
                transform.localPosition = initialPosition;
                shakeMagnitude = baseShakeMagnitude;
                dampingSpeed = baseDampingSpeed;
            }
        }
    }

    // Starts camera shake
    public void TriggerShake(float duration)
    {
        shakeDuration += duration;
    }

    // Starts camera shake
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration += duration;
        shakeMagnitude = magnitude;
    }

    // Starts camera shake
    public void TriggerShake(float duration, float magnitude, float damping)
    {
        shakeDuration += duration;
        shakeMagnitude = magnitude;
        dampingSpeed = damping;
    }
}
