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
    private Vector3 targetRot;
    [SerializeField]
    private float lockedFollowSpeed = 5.0f;
    [SerializeField]
    private float fovLerpSpeed = 3.0f;
    [SerializeField]
    private float angleLerpSpeed = 3.0f;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private DungeonRoom room;
    [SerializeField]
    private Vector3 roomPos;
    [SerializeField]
    private Vector2 roomSize;
    [SerializeField]
    private Vector2 defaultRoomSize;

    [SerializeField]
    private float targetFov = 0.0f;
    private const float FOCAL_LENGTH_MOD = 2.95f;
    private const float OFFSET_X_MOD = -0.2f;
    private const float OFFSET_B_MOD = -2.5f;

    private void Start()
    {
        shake = Camera.main.GetComponentInParent<CameraShake>();
        //destination = new Vector3(roomPos.x + offset.x, offset.y, roomPos.z + offset.z);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetFov = GetTargetFOV();
    }

    private void Update()
    {
        // Lerp camera to target FOV
        //if(!MathUtils.AlmostZero(Camera.main.fieldOfView - targetFov, 2))
        //    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFov, fovLerpSpeed * Time.deltaTime);

        // Update destination to be player if in a large room
        if (IsLargeRoom())
            destination = ClampDestinationToRoomBounds(player.position);

        // Follow with camera to destination
        if (MathUtils.AlmostZero(this.transform.position - destination, 2))
            return;

        /*
        Vector3 tempVec = Vector3.Lerp(this.transform.position, destination, lockedFollowSpeed * Time.deltaTime);
        this.transform.position = new Vector3(tempVec.x, transform.position.y, tempVec.z);
        this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, targetRot, angleLerpSpeed * Time.deltaTime);
        */
    }

    // Set the room to follow with the camera
    public void SetRoom(DungeonRoom newRoom)
    {
        // Don't set the room to follow to null
        if (newRoom == null)
            return;

        room = newRoom;
        roomSize = room.GetSize();
        roomPos = room.GetPosition();
        //offset.z = OFFSET_X_MOD * roomSize.x + OFFSET_B_MOD;

        // Position the camera to match the room type
        if(IsLargeRoom())
        {
            destination = new Vector3(roomPos.x, offset.y, roomPos.z);
            targetRot = new Vector3(90f, 0f, 0f);
        }
        else
        {
            destination = new Vector3(roomPos.x + offset.x, offset.y, roomPos.z + offset.z);
            targetRot = new Vector3(60f, 0f, 0f);
        }
        targetFov = GetTargetFOV();
    }

    // Returns the destination clamped to the bounds that the camera can move to
    private Vector3 ClampDestinationToRoomBounds(Vector3 destination)
    {
        return new Vector3(
            Mathf.Clamp(destination.x, 
                roomPos.x - roomSize.x * 0.5f - defaultRoomSize.x * 0.5f + offset.x, 
                roomPos.x + roomSize.x * 0.5f + defaultRoomSize.x * 0.5f + offset.x), 
            Mathf.Clamp(destination.y, 
                offset.y, 
                offset.y), 
            Mathf.Clamp(destination.z, 
                roomPos.z - roomSize.y * 0.5f - defaultRoomSize.y * 0.5f,
                roomPos.z + roomSize.y * 0.5f + defaultRoomSize.y * 0.5f));
    }

    // Returns whether the player is in a big room or not
    private bool IsLargeRoom() { return Mathf.Max(roomSize.x, roomSize.y) > defaultRoomSize.x; }

    // Returns the targetFOV
    private float GetTargetFOV()
    {
        return 2 * Mathf.Atan(Mathf.Max(roomSize.x, roomSize.y) * FOCAL_LENGTH_MOD / (2 * Camera.main.focalLength)) * Mathf.Rad2Deg; ;
    }

    // Shakes the camera
    public void Shake(float duration = 0.5f, float magnitude = 0.7f, float damping = 1.0f)
    {
        shake.TriggerShake(duration, magnitude, damping);
    }
}
