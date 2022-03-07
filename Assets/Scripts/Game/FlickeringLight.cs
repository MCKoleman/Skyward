using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
	[SerializeField]
	private float minIntensity = 0.25f;
	[SerializeField]
	private float maxIntensity = 0.5f;

	float random;

	void Start()
	{
		random = Random.Range(0.0f, 65535.0f);
	}

	void Update()
	{
		float noise = Mathf.PerlinNoise(random, Time.time);
		GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
	}
}
