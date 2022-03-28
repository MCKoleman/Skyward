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
    private DungeonRoom room;
    [SerializeField]
    private Vector3 roomPos;
    [SerializeField]
    private Vector2 roomSize;

    [SerializeField]
    private float targetFov = 0.0f;
    private const float FOCAL_LENGTH_MOD = 2.95f;
    private const float OFFSET_X_MOD = -0.2f;
    private const float OFFSET_B_MOD = -2.5f;
    private const float MIN_LARGE_ROOM = 15.0f;

    private void Start()
    {
        shake = Camera.main.GetComponentInParent<CameraShake>();
        destination = new Vector3(roomPos.x + offset.x, offset.y, roomPos.z + offset.z);
        targetFov = GetTargetFOV();
        //Print.Log($"Camera bounds: [{Camera.main.ScreenToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f))}], [{Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f))}]");
        //cameraSpace = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f)) - Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
    }

    private void Update()
    {
        // Lerp camera to target FOV
        if(!MathUtils.AlmostZero(Camera.main.fieldOfView - targetFov, 2))
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFov, fovLerpSpeed * Time.deltaTime);

        // Follow with camera to destination
        if (MathUtils.AlmostZero(this.transform.position - destination, 2))
            return;

        Vector3 tempVec = Vector3.Lerp(this.transform.position, destination, lockedFollowSpeed * Time.deltaTime);
        this.transform.position = new Vector3(tempVec.x, transform.position.y, tempVec.z);
        this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, targetRot, angleLerpSpeed * Time.deltaTime);

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
        roomSize = room.GetSize();
        roomPos = room.GetPosition();
        //offset.z = OFFSET_X_MOD * roomSize.x + OFFSET_B_MOD;

        // Position the camera to match the room type
        if(Mathf.Max(roomSize.x, roomSize.y) > MIN_LARGE_ROOM)
        {
            destination = new Vector3(roomPos.x + offset.x, offset.y, roomPos.z);
            targetRot = new Vector3(90f, 0f, 0f);
        }
        else
        {
            destination = new Vector3(roomPos.x + offset.x, offset.y, roomPos.z + offset.z);
            targetRot = new Vector3(60f, 0f, 0f);
        }
        targetFov = GetTargetFOV();
    }

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
