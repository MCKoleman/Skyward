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
    private bool isLockedToPlayer = true;
    [SerializeField]
    private DungeonRoom room;
    [SerializeField]
    private Vector3 roomCoords;
    [SerializeField]
    private Vector3 roomSize;
    [SerializeField]
    private GameObject objToFollow;

    private void Start()
    {
        objToFollow = GameObject.FindGameObjectWithTag("Player");
        shake = Camera.main.GetComponentInParent<CameraShake>();
        //Print.Log($"Camera bounds: [{Camera.main.ScreenToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f))}], [{Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f))}]");
        //cameraSpace = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f)) - Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
    }

    private void Update()
    {
        // Follow with camera to destination
        if (MathUtils.AlmostZero(this.transform.position - destination, 2))
            return;

        Vector3 tempVec = Vector3.Lerp(this.transform.position, destination, lockedFollowSpeed * Time.deltaTime);
        this.transform.position = new Vector3(tempVec.x, transform.position.y, tempVec.z);
        Camera.main.fieldOfView = 2 * Mathf.Atan(roomSize.x / (2 * Camera.main.focalLength)) * Mathf.Rad2Deg;

        /*
        // If the camera view is locked to the room, follow locked rules
        if(isLockedToRoom)
        {
            UpdateDestination();
            Vector3 tempVec = Vector3.Lerp(transform.position, destination + offset, lockedFollowSpeed * Time.deltaTime);
            this.transform.position = new Vector3(tempVec.x, transform.position.y, tempVec.z);

            // TEMP CODE
            // Rotate camera to face player position
            if(isLockedToPlayer)
            {
                Vector3 distanceToPlayer = this.transform.position - objToFollow.transform.position;
                float angleX = (distanceToPlayer.x != 0) ? Mathf.Atan2(distanceToPlayer.y, distanceToPlayer.x) : 0;
                float angleY = (distanceToPlayer.z != 0) ? Mathf.Atan2(distanceToPlayer.y, distanceToPlayer.z) : 0;
                Print.Log($"Angles to player: [{angleX}], [{angleY}]");
                this.transform.eulerAngles = new Vector3(angleX * Mathf.Rad2Deg, angleY * Mathf.Rad2Deg, 0.0f);
                //Print.Log($"Distance between camera and player: [{distance}]");
            }
        }
        // If the camera is not locked to the room, follow the player
        else if(objToFollow != null)
        {
            Vector3 tempVec = Vector3.Lerp(transform.position, objToFollow.transform.position + offset, followSpeed * Time.deltaTime);
            transform.position = new Vector3(tempVec.x, transform.position.y, tempVec.z);
        }
        */
    }

    // Set the room to follow with the camera
    public void SetRoom(DungeonRoom newRoom)
    {
        // Don't set the room to follow to null
        if (newRoom == null)
            return;

        room = newRoom;
        roomSize = newRoom.CalcSize();
        roomCoords = new Vector3(newRoom.roomPos.x, 1.0f, newRoom.roomPos.y);
        destination = new Vector3(roomCoords.x + offset.x, offset.y, roomCoords.z + offset.z);
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

    // Shakes the camera
    public void Shake(float duration = 0.5f, float magnitude = 0.7f, float damping = 1.0f)
    {
        shake.TriggerShake(duration, magnitude, damping);
    }
}
