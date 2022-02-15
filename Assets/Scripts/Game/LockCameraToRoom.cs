using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCameraToRoom : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private float followSpeed = 1.5f;
    [SerializeField]
    private float lockedFollowSpeed = 5.0f;
    [SerializeField]
    private bool isLockedToRoom = true;
    [SerializeField]
    private Vector3 roomCoords;
    [SerializeField]
    private Vector3 roomSize;
    [SerializeField]
    private GameObject objToFollow;

    private void Start()
    {
        objToFollow = GameObject.FindGameObjectWithTag("Player");
        //Print.Log($"Camera bounds: [{Camera.main.ScreenToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f))}], [{Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f))}]");
        //cameraSpace = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f)) - Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
    }

    private void Update()
    {
        // If the camera view is locked to the room, follow locked rules
        if(isLockedToRoom)
        {
            UpdateDestination();
            Vector3 tempVec = Vector3.Lerp(transform.position, destination + offset, lockedFollowSpeed * Time.deltaTime);
            transform.position = new Vector3(tempVec.x, transform.position.y, tempVec.z);
        }
        // If the camera is not locked to the room, follow the player
        else if(objToFollow != null)
        {
            Vector3 tempVec = Vector3.Lerp(transform.position, objToFollow.transform.position + offset, followSpeed * Time.deltaTime);
            transform.position = new Vector3(tempVec.x, transform.position.y, tempVec.z);
        }
    }

    // Returns the coordinates of the room that the target is currently in (in room space)
    private Vector3Int GetRoomCoordinates()
    {
        return new Vector3Int(Mathf.FloorToInt(objToFollow.transform.position.x / roomSize.x + 0.5f), 
            0,
            Mathf.FloorToInt(objToFollow.transform.position.z / roomSize.z + 0.5f));
    }

    // Updates the target destination to be the current room
    private void UpdateDestination()
    {
        if(objToFollow != null)
        {
            roomCoords = GetRoomCoordinates();
            destination = new Vector3(roomCoords.x * roomSize.x, transform.position.y, roomCoords.z* roomSize.z);
        }
    }
}
