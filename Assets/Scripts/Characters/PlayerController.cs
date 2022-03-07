using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController
{
    /* ==================================================== Variables =================================================================== */
    [Header("Player Controller data")]
    [SerializeField]
    protected AudioClip[] deathClips;

    /* Component references*/
    protected List<Interactable> interactables;
    protected CameraController cam;

    protected float rayLength;
    protected Ray cameraRay;
    protected Plane gamePlane;

    protected MeleeAttack mAttack;


    /* ==================================================== Built-in functions =================================================================== */
    protected override void Start()
    {
        base.Start();
        interactables = new List<Interactable>();
        cam = this.GetComponent<CameraController>();

        gamePlane = new Plane(Vector3.up, Vector3.zero);
    }

    protected void Update()
    {

    }

    /* ==================================================== Helper functions =================================================================== */
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

    // Cast ray from top-down camera onto imaginary plane
    protected void FaceMousePos()
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

    /* ==================================================== Input actions =================================================================== */
    protected void HandleAttack()
    {

    }
    
    protected void HandleFire()
    {

    }

    protected void HandleCrouch()
    {

    }

    protected void HandleDash()
    {
        // Only allow dash if the dash cooldown has finished and the player has dashes left
        if(HasDashesLeft() && !isDashing)
        {
            // Increase dash counter only when midair
            if(!isGrounded)
                numDashesUsed++;

            // Start coroutine to handle the dash motion
            StartCoroutine(HandleDashMotion());
        }
    }

    protected void HandleAbility()
    {
        // TODO: Handle ability
    }

    protected void HandleInteract()
    {
        // TODO: Handle interacting with objects
    }

    protected void HandleMenu()
    {
        // TEMP: QUIT GAME ON EXIT INSTEAD OF PAUSE
        GameManager.Instance.QuitGame();
        //UIManager.Instance.PauseGameToggle();
    }



    /* ==================================================== Input context handlers =================================================================== */
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        //Debug.Log($"Can move? : [{GameManager.Instance.IsGameActive}]");
        if(CanTakeInput())
            HandleMove(context.ReadValue<Vector2>());
    }

    public void HandleFireContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleFire();
    }

    public void HandleCrouchContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleCrouch();
    }

    public void HandleDashContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleDash();
    }

    public void HandleAbilityContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleAbility();
    }

    public void HandleInteractContext(InputAction.CallbackContext context)
    {
        if (context.performed && CanTakeInput())
            HandleInteract();
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
