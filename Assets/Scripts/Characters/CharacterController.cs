using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /* ==================================================== Variables =================================================================== */
    [Header("Base Character Controller data")]
    /* Inspector data */
    [SerializeField, Range(0, 10)]
    protected int maxNumJumps = 2;
    [SerializeField, Range(0, 10)]
    protected int maxNumDashes = 2;

    /* State data */
    /* Animation data */
    protected bool isGrounded = false;
    protected bool isWalking = false;
    protected bool isDashing = false;

    /* Movement data */
    protected float facingDir = 1.0f;
    protected Vector3 moveDelta = Vector3.zero;
    protected Vector3 prevDelta = Vector3.zero;
    protected int numJumpsUsed = 0;
    protected int numDashesUsed = 0;

    /* Component references */
    protected Rigidbody rb;
    protected Collider coll;
    protected Animator anim;
    protected Character character;
    protected AudioSource source;

    [SerializeField]
    protected AudioClip[] jumpClips;
    [SerializeField]
    protected AudioClip[] dashClips;

    /* Constants */
    protected const float MAX_SLOPE_ANGLE = 45.0f;              // Maximum angle that the character can land on a surface at and be considered on the ground
    protected const float COLLISION_IGNORE_TIME = 0.5f;         // Time between dropping off a platform that collisions are ignored for
    protected const float MIDAIR_ACCELERATION_MOD = 0.5f;       // Multiplier to the character's acceleration while they are midair
    protected const float MIDAIR_DECELERATION_MOD = 0.5f;       // Multiplier to the character's deceleration while they are midair
    protected const float SCALE_FLIP_MOD = 8.0f;                // Multiplier for the speed at which the character scales when flipping around
    protected const float MINIMUM_GROUNDED_DROP = -0.3f;        // Minimum y velocity for the character to have before they are no longer considered grounded
    public CharacterSpeedMods speedMods;

    /* Cooldowns */
    [SerializeField, Range(0.0f, 10.0f)]
    protected float maxJumpCooldown = 0.2f;
    protected float curJumpCooldown = 0.0f;
    protected float maxAirMovementCooldown = 0.1f;
    protected float airMovementCooldown = 0.0f;


    /* Ground detection */
    protected List<GameObject> groundObjs;

    /* ==================================================== Built-in functions =================================================================== */
    protected virtual void Start()
    {
        // Init component references
        rb = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
        coll = this.GetComponent<Collider>();
        character = this.GetComponent<Character>();
        source = this.GetComponent<AudioSource>();
        groundObjs = new List<GameObject>();

        ResetAllCooldowns();
    }

    protected virtual void FixedUpdate()
    {
        // If there is movement input being given, move the rigidbody in the direction of the delta over time
        if (moveDelta != Vector3.zero && !isDashing)
            //rb.MovePosition(rb.position + moveDelta * Time.fixedDeltaTime * Vector2.right);
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(moveDelta.x, rb.velocity.y, moveDelta.z), GetAccelerationMod(true) * Time.fixedDeltaTime);
        else
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0.0f, rb.velocity.y, 0.0f), GetAccelerationMod(false) * Time.fixedDeltaTime);

        // Decrement air cooldown
        if (airMovementCooldown > 0.0f && !isGrounded && MathUtils.AlmostZero(rb.velocity.y, 2))
            airMovementCooldown -= Time.fixedDeltaTime;
        else if (!isGrounded && !MathUtils.AlmostZero(rb.velocity.y, 2))
            airMovementCooldown = maxAirMovementCooldown;

        // If the player falls, set grounded to false
        if (isGrounded && rb.velocity.y < MINIMUM_GROUNDED_DROP)
            SetIsGrounded(false);
        // If the player is on the ground for an extended time period, reset their grounded status
        else if (!isGrounded && MathUtils.AlmostZero(rb.velocity.y, 2) && airMovementCooldown <= 0.0f)
        {
            //Print.Log("Ground detection failed, fixing it...");
            SetIsGrounded(true);
        }

        // Handle cooldowns
        HandleCooldowns();
    }



    /* ==================================================== Animation state functions =================================================================== */

    // Sets the isDashing variable in both state and anim to the given value
    public void SetIsDashing(bool value)
    {
        isDashing = value;
        anim?.SetBool("isDashing", value);
    }

    // Sets the isWalking variable in both state and anim to the given value
    public void SetIsWalking(bool value)
    {
        isWalking = value;
        anim?.SetBool("isWalking", value);
    }

    // Sets the isGrounded variable in both state and anim to the given value
    public void SetIsGrounded(bool value)
    {
        isGrounded = value;
        anim?.SetBool("isGrounded", value);
    }



    /* ==================================================== Helper functions =================================================================== */
    // Returns the acceleration modifier for moving
    protected float GetAccelerationMod(bool acceleration)
    {
        // If the player is grounded, only use speed and slow mods. If the player is midair, also apply air accel/decel mods
        return (acceleration ? speedMods.MOVE_SPEED_MOD : speedMods.MOVE_SLOW_MOD) * (isGrounded ? 1.0f : (acceleration ? MIDAIR_ACCELERATION_MOD : MIDAIR_DECELERATION_MOD));
    }
    
    // Returns whether the player has enough jumps left to perform another jump
    public bool HasJumpsLeft()
    {
        return (numJumpsUsed < maxNumJumps);
    }

    // Returns whether the player has enough dashes left to perform another dash
    public bool HasDashesLeft()
    {
        return (numDashesUsed < maxNumDashes);
    }

    // Resets all cooldowns of this character
    protected virtual void ResetAllCooldowns()
    {
        curJumpCooldown = maxJumpCooldown;
    }

    // Handles the tick of each cooldown (handled in fixed delta time)
    protected virtual void HandleCooldowns()
    {
        if (curJumpCooldown > 0.0f)
            curJumpCooldown = Mathf.Max(curJumpCooldown - Time.fixedDeltaTime, 0.0f);
    }

    // Handles the movement based on Vector2 directional input
    protected virtual void HandleMove(Vector2 moveContext)
    {
        // If the movement hasn't ended, store it as the previous delta for direction calculations
        if(moveDelta != Vector3.zero)
            prevDelta = new Vector3(moveDelta.x, 0.0f, moveDelta.z);

        // Get the movement delta for each frame from the x and y axis input
        moveDelta = new Vector3(moveContext.x, 0.0f, moveContext.y) * speedMods.MOVE_MOD;

        // Set animation state
        SetIsWalking((moveDelta != Vector3.zero));
    }

    // Handles the jumping of the character
    protected virtual void HandleJump()
    {
        // Only allow jump if player is on the ground or has an unused double jump
        if ((isGrounded || HasJumpsLeft()) && curJumpCooldown <= 0.0f && !isDashing)
        {
            // Reset velocity before jumping
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(speedMods.JUMP_MOD * Vector3.up, ForceMode.Impulse);

            // Play jump sound
            if(source != null && jumpClips.Length != 0)
            {
                source.clip = jumpClips[Random.Range(0, jumpClips.Length)];
                source.Play();
            }

            // Increment number of jumps to prevent illegal jumping
            numJumpsUsed++;
            curJumpCooldown = maxJumpCooldown;
            SetIsGrounded(false);
        }
    }

    // Handles the dash motion as a coroutine, pausing regular movement for the duration of the dash
    protected IEnumerator HandleDashMotion()
    {
        float timeLeft = speedMods.DASH_DURATION;
        SetIsDashing(true);

        // Freeze the player before dashing
        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        // Activate dash
        rb.AddForce(prevDelta.normalized * speedMods.DASH_MOD, ForceMode.Impulse);

        // Play dash sound
        if (source != null && dashClips.Length != 0)
        {
            source.clip = dashClips[Random.Range(0, dashClips.Length)];
            source.Play();
        }

        // Move the player steadily
        while (timeLeft > 0.0f)
        {
            yield return new WaitForFixedUpdate();
            timeLeft -= Time.fixedDeltaTime;
        }

        // Reset movement after dash
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        SetIsDashing(false);
    }



    /* =========================================================== Collisions ============================================================================== */
    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collided with [{collision.gameObject.name}] at [{collision.GetContact(0).point}]");
        // Handle ground collision
        // Reset consecutive jumps on collision with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Only reset jumps if the player hit ground from above
            if (Vector3.Angle(Vector3.up, collision.GetContact(0).normal) < MAX_SLOPE_ANGLE)
            {
                // Ground the player
                numJumpsUsed = 0;
                numDashesUsed = 0;
                SetIsGrounded(true);
            }
        }
    }
}
