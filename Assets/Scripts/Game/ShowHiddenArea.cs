using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowHiddenArea : MonoBehaviour
{
    // Components
    private Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;

    // Info
    private int originalRenderOrder;
    [SerializeField]
    private Color shownColor;
    [SerializeField]
    private Color hiddenColor;
    [SerializeField]
    private float colorLerpMod = 3.0f;

    private bool isLerping = false;

    private void Start()
    {
        // Grab the tilemap
        if (tilemap == null)
            tilemap = this.GetComponent<Tilemap>();

        // Grab the tilemap renderer
        if (tilemapRenderer == null)
            tilemapRenderer = this.GetComponent<TilemapRenderer>();

        originalRenderOrder = tilemapRenderer.sortingOrder;
    }

    // Shows the area
    public void ShowArea()
    {
        StopAllCoroutines();
        isLerping = false;
        StartCoroutine(LerpToColor(shownColor, 0));
    }
    
    // Hides the area
    public void HideArea()
    {
        StopAllCoroutines();
        isLerping = false;
        StartCoroutine(LerpToColor(hiddenColor, originalRenderOrder));
    }

    // Linearly interpolates from the current color to the given color
    private IEnumerator LerpToColor(Color color, int renderOrder)
    {
        isLerping = true;
        // Keep looping until the correct color is achieved
        while (tilemap.color != color && isLerping)
        {
            // Lerp to color
            tilemap.color = Vector4.Lerp(tilemap.color, color, Time.deltaTime * colorLerpMod);

            // Wait for frame
            yield return new WaitForEndOfFrame();

            // When colors are almost equal, exit by making them the same
            if (MathUtils.IsAlmostColor(tilemap.color, color))
            {
                tilemap.color = color;
                break;
            }
        }
        tilemapRenderer.sortingOrder = renderOrder;
        isLerping = false;
    }

    // Show area when entering
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
            ShowArea();
    }

    // Hide area when leaving
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
            HideArea();
    }
}
