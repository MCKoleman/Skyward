using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : CharacterController
{
    /* ==================================================== Variables =================================================================== */
    [Header("Player Controller data")]
    [SerializeField]
    protected AudioClip[] deathClips;
    [SerializeField]
    protected float rotationLerpSpeed = 5.0f;

    /* Component references*/
    protected List<Interactable> interactables;
    protected CameraController cam;
    [SerializeField]
    protected PlayerAttack_Melee meleeAttack;
    private Plane rotationPlane;

    [SerializeField]
    private float targetRotation = 0.0f;
    protected GlobalVars.AbilityType curAbility;

    /* ==================================================== Built-in functions =================================================================== */
    protected override void Start()
    {
        base.Start();
        interactables = new List<Interactable>();
        cam = Camera.main.GetComponent<CameraController>();

        rotationPlane = new Plane(Vector3.up, Vector3.zero);
    }

    protected void Update()
    {
        // If the rotation is already close to correct, set it to be the exact desired rotation
        if (DoesMatchRotation())
        {
            this.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);
            return;
        }

        // Rotate player to face look direction
        float tempRot = Mathf.Lerp(0.0f, GetNormalizedAngleDifference(), rotationLerpSpeed * Time.deltaTime);
        this.transform.Rotate(new Vector3(0.0f, tempRot, 0.0f));
    }

    /* ==================================================== Helper functions =================================================================== */
    // Getters and setters for ability type
    public GlobalVars.AbilityType GetActiveAbility() { return curAbility; }
    public void SetActiveAbility(GlobalVars.AbilityType _type) { curAbility = _type; }
    
    // Returns whether the rotation of the character matches the target rotation
    private bool DoesMatchRotation()
    {
        // Normalize target rotation
        if (targetRotation < 0.0f)
            targetRotation += 360.0f;

        return MathUtils.AlmostZero(GetRotationAngleDifference(), 1);
    }

    // Normalizes the angle difference given
    private float NormalizeAngleDif(float angleDif)
    {
        // Fix angle differences
        if (angleDif <= -180.0f)
            return angleDif + 360.0f;
        else if (angleDif >= 180.0f)
            return angleDif - 360.0f;
        else
            return angleDif;
    }

    // Returns the normalized angle difference between current and target rotations
    private float GetNormalizedAngleDifference() { return NormalizeAngleDif(GetRotationAngleDifference()); }

    // Returns the angle difference between the current and target rotations
    private float GetRotationAngleDifference() { return targetRotation - this.transform.rotation.eulerAngles.y; }
    
    // Returns whether the player is colliding with the given interactable type
    protected bool CollidingWithInteractable(Interactable.InteractableType iType)
    {
        // Checks the list for any matches to the given interactable type
        for (int i = 0; i < interactables.Count; i++)
            if (interactables[i].type == iType)
                return true;

        // If there were no matches to the given interactable type, return false
        return false;
    }

    // Returns the first interactable of the given type
    protected Interactable GetFirstInteractableOfType(Interactable.InteractableType iType)
    {
        // If the interactable is found, return it
        for (int i = 0; i < interactables.Count; i++)
            if (interactables[i].type == iType)
                return interactables[i];

        // If none is found, return null
        return null;
    }

    // Handles resetting the player information during the respawn process
    public void RespawnPlayer()
    {
        // Play death sound in temp channel in audioManager
        //AudioManager.Instance.PlayClip(deathClips[Random.Range(0,deathClips.Length)]);

        // Handle player death info
        //SaveManager.Instance.IncDeaths();
    }

    // Returns whether the player can take input or not
    public bool CanTakeInput()
    {
        return GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused();
    }

    // Handles winning the level
    public void WinLevel()
    {
        // Reset movement
        GameManager.Instance.SetIsGameActive(false);
        rb.velocity = Vector3.zero;
        moveDelta = Vector2.zero;

        // Handle UI
        //UIManager.Instance.ShowVictoryScreen(true);
        //AudioManager.Instance.PlayVictoryMusic();
        //GameManager.Instance.HandleLevelSwap(tempI.value);
        SaveManager.Instance.WinLevel();
    }



    /* ==================================================== UI Functions =================================================================== */
    protected override void SetDashCooldown(float _val)
    {
        base.SetDashCooldown(_val);
        UIManager.Instance.UpdateDashCooldown(GetDashCooldownPercent());
    }

    protected override void SetJumpCooldown(float _val)
    {
        base.SetJumpCooldown(_val);
    }



    /* ==================================================== Input actions =================================================================== */
    protected void HandleLook(Vector2 lookPos)
    {
        // Don't look outside of screen
        Vector3 viewPort = Camera.main.ScreenToViewportPoint(lookPos);
        if (viewPort.x < 0.0f || viewPort.y < 0.0f || viewPort.x > 1.0f || viewPort.y > 1.0f)
            return;

        Ray viewRay = Camera.main.ScreenPointToRay(lookPos);
        float rayLength;
        if (rotationPlane.Raycast(viewRay, out rayLength))
        {
            // Rotate Player forward vector to face mouse position (point of intersection)
            Vector3 lookDif = viewRay.GetPoint(rayLength) - this.transform.position;
            targetRotation = Vector2.SignedAngle(new Vector2(lookDif.x, lookDif.z), Vector2.up);
        }
    }

    protected void HandleLookDelta(Vector2 lookDelta)
    {
        // Don't change the look direction to nothing
        if(lookDelta != Vector2.zero && lookDelta.magnitude > 0.1f)
        {
            targetRotation = Vector2.SignedAngle(lookDelta, Vector2.up);
        }
    }

    protected void HandleAttack()
    {
        meleeAttack.Attack();
    }

    protected void HandleDash()
    {
        // Only allow dash if the dash cooldown has finished and the player has dashes left
        if((isGrounded || HasDashesLeft()) && curDashCooldown <= 0.0f && !isDashing)
        {
            // Increase dash counter only when midair
            if(!isGrounded)
                numDashesUsed++;

            // Start coroutine to handle the dash motion
            StartCoroutine(HandleDashMotion());
        }
    }

    protected void HandleSpell()
    {
        // TODO: Handle spell
        switch(curAbility)
        {
            case GlobalVars.AbilityType.MAGIC_MISSILE:
                // Cast magic missiles
                break;
            case GlobalVars.AbilityType.METEOR:
                // Cast meteor
                break;
            case GlobalVars.AbilityType.ICE_WAVE:
                // Cast ice wave
                break;
            case GlobalVars.AbilityType.LIGHTNING_BOLT:
                // Cast lightning bolt
                break;
            case GlobalVars.AbilityType.DEFAULT:
            default:
                break;
        }
    }

    protected void HandleShield()
    {
        // TODO: Handle shield
    }

    protected void HandleAbility1()
    {
        curAbility = UIManager.Instance.SelectAbility(GlobalVars.AbilityType.METEOR, curAbility);
    }

    protected void HandleAbility2()
    {
        curAbility = UIManager.Instance.SelectAbility(GlobalVars.AbilityType.ICE_WAVE, curAbility);
    }

    protected void HandleAbility3()
    {
        curAbility = UIManager.Instance.SelectAbility(GlobalVars.AbilityType.LIGHTNING_BOLT, curAbility);
    }

    protected void HandleMenu()
    {
        UIManager.Instance.PauseGameToggle();
    }



    /* ==================================================== Input context handlers =================================================================== */
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        //Debug.Log($"Can move? : [{GameManager.Instance.IsGameActive}]");
        if(CanTakeInput())
            HandleMove(context.ReadValue<Vector2>());
    }

    public void HandleLookContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput())
            HandleLook(context.ReadValue<Vector2>());
    }

    public void HandleLookDeltaContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput())
            HandleLookDelta(context.ReadValue<Vector2>());
    }

    public void HandleAttackContext(InputAction.CallbackContext context)
    {
        // Prevent attacking if the player has their mouse over UI elements
        if (context.performed && CanTakeInput() && !EventSystem.current.IsPointerOverGameObject())
            HandleAttack();
    }

    public void HandleSpellContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput() && !EventSystem.current.IsPointerOverGameObject())
            HandleSpell();
    }

    public void HandleDashContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleDash();
    }

    public void HandleShieldContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleShield();
    }

    public void HandleAbility1Context(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbility1();
    }

    public void HandleAbility2Context(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbility2();
    }

    public void HandleAbility3Context(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbility3();
    }

    public void HandleJumpContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleJump();
    }

    public void HandleMenuContext(InputAction.CallbackContext context)
    {
        // Allow input when paused, but not when game is inactive
        if (context.performed && GameManager.Instance.IsGameActive)
            HandleMenu();
    }
    public void HandleSkipContext(InputAction.CallbackContext context)
    {
        // Turn on and off dialogue skipping
        if (context.performed && GameManager.Instance.IsGameActive)
            DialogueManager.Instance.EnableFastDialogue(true);
        else if (context.canceled)
            DialogueManager.Instance.EnableFastDialogue(false);
    }



    /* ==================================================== Collision Triggers =================================================================== */
    protected void OnTriggerEnter(Collider collision)
    {
        HandleTriggerEnter(collision, true);
    }

    protected void OnTriggerExit(Collider collision)
    {
        HandleTriggerExit(collision, true);
    }



    /* ==================================================== Collision Handlers =================================================================== */
    // Handles trigger enter collisions
    public void HandleTriggerEnter(Collider collision, bool isReal = true)
    {
        //Debug.Log($"Called HandleTriggerEnter() with {isReal}");
        // Only handle the collisions if it is real
        if(isReal)
        {
            // Add interactable to the list
            Interactable tempI = collision.GetComponent<Interactable>();
            if (tempI != null)
            {
                interactables.Add(tempI);

                // Handle interactables
            }
        }
    }

    // Handles trigger exit collisions
    public void HandleTriggerExit(Collider collision, bool isReal = true)
    {
        //Debug.Log($"Called HandleTriggerEnter() with {isReal}");
        // Remove interactable from the list
        Interactable tempI = collision.GetComponent<Interactable>();
        if (tempI != null)
            interactables.Remove(tempI);
    }
}
