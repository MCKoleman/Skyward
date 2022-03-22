using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 dungeonCenter;
    [SerializeField]
    private float targetWidth;
    [SerializeField]
    private bool isPlayerView = true;
    [SerializeField]
    private float lerpSpeed = 3.0f;
    [SerializeField]
    private float zoom = 1.0f;

    private const float DEFAULT_ZOOM_MOD = 10.0f;
    private GameObject player;
    private Camera cam;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (cam == null)
            cam = this.GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // Focus on player
        if (isPlayerView)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, lerpSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom * DEFAULT_ZOOM_MOD, lerpSpeed * Time.deltaTime);
        }
        // Focus on entire map
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, dungeonCenter + offset, lerpSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetWidth, lerpSpeed * Time.deltaTime);
        }
    }

    // Sets the display center of the camera
    public void SetCameraWidth(float _width) { targetWidth = _width; }

    // Sets the focus point of the minimap camera when in dungeon view
    public void SetDungeonCenter(Vector3 _center) { dungeonCenter = _center; }

    // Sets the display mode of the minimap camera
    public void SetDisplayMode(bool _isPlayerView) { isPlayerView = _isPlayerView; }

    // Toggles the display mode of the dungeon
    public void ToggleDisplayMode() { isPlayerView = !isPlayerView; }

    // Returns whether the minimap is in player display mode or not
    public bool GetIsPlayerView() { return isPlayerView; }
}
