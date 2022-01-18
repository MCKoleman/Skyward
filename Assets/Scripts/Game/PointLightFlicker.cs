using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.Rendering.Universal;

public class PointLightFlicker : MonoBehaviour
{
    // Inspector info
    [Header("Flicker Info")]
    [SerializeField, Range(0.01f, 3.0f), Tooltip("Minimum time that the light is at its lowest intensity")]
    private float minFlicker = 0.1f;
    [SerializeField, Range(0.01f, 3.0f), Tooltip("Maximum time that the light is at its lowest intensity")]
    private float maxFlicker = 0.2f;
    [SerializeField, Range(0.01f, 60.0f), Tooltip("Minimum time between flickers")]
    private float minWaitTime = 12.0f;
    [SerializeField, Range(0.01f, 60.0f), Tooltip("Maximum time between flickers")]
    private float maxWaitTime = 20.0f;
    [SerializeField, Range(0.01f, 2.0f), Tooltip("Factor of intensity of light during flicker")]
    private float lowIntensityFactor = 0.5f;
    [SerializeField, Range(0.01f, 50.0f), Tooltip("Factor by which the intensity is smooted into the desired intensity")]
    private float flickerSmoothFactor = 10.0f;

    // Component info
    private float defaultIntensity;
    private float lowIntensity;
    private Light lightRef;

    void Start()
    {
        // Get info of light
        lightRef = this.GetComponent<Light>();
        defaultIntensity = lightRef.intensity;
        lowIntensity = defaultIntensity * lowIntensityFactor;

        // Start looping through flicker
        StartCoroutine(LoopFlicker());
    }

    // Coroutine for continuously flickering the light in the desired manner
    private IEnumerator LoopFlicker()
    {
        while(true)
        {
            // Calculate times
            float tempWait = Random.Range(minWaitTime, maxWaitTime);
            float tempFlicker = Random.Range(minFlicker, maxFlicker) / 2;

            // Wait to flicker
            yield return new WaitForSeconds(tempWait);

            // Smooth intensity to low
            yield return StartCoroutine(LerpIntensity(lowIntensity, tempFlicker));

            // Smooth intensity back to default
            yield return StartCoroutine(LerpIntensity(defaultIntensity, tempFlicker));
        }
    }

    private IEnumerator LerpIntensity(float targetIntensity, float time)
    {
        float tempTime = 0.0f;
        // Keep lerping until the target intensity is reached
        while(lightRef.intensity != targetIntensity && tempTime < time)
        {
            lightRef.intensity = Mathf.Lerp(lightRef.intensity, targetIntensity, flickerSmoothFactor * Time.fixedDeltaTime);
            tempTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
