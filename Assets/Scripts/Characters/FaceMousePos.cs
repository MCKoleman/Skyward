using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMousePos : MonoBehaviour
{
    private float rayLength;
    private Ray cameraRay;
    private Plane gamePlane;

    // Start is called before the first frame update
    void Start()
    {
        gamePlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        // Cast ray from top-down camera onto mouse's x/y position
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // When ray intersects imaginary plane representing the game space
        if (gamePlane.Raycast(cameraRay, out rayLength))
        {
            // Rotate Player forward vector to face mouse position (point of intersection)
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
