using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarClear : MonoBehaviour
{
    [SerializeField]
    private bool isRevealed = false;
    [SerializeField, Range(0.0f, 1.0f)]
    private float curAlpha = 1.0f;
    [SerializeField, Range(0.1f, 10.0f)]
    private float revealSpeed = 3.0f;

    [SerializeField]
    private Renderer roomRenderer;
    [SerializeField]
    private Renderer minimapRenderer;

    private void Update()
    {
        // Don't do anything until this room is revealed
        if (!isRevealed)
            return;

        // Lerp alpha to clear
        curAlpha = Mathf.Lerp(curAlpha, 0.0f, revealSpeed * Time.deltaTime);
        minimapRenderer.material.SetFloat("_Alpha", curAlpha);
        roomRenderer.material.SetFloat("_Alpha", curAlpha);

        // Disable the fog of war completely when revealed
        if (MathUtils.AlmostZero(curAlpha))
            this.gameObject.SetActive(false);
    }

    // Reveals this room
    public void Reveal()
    {
        isRevealed = true;
    }
}
