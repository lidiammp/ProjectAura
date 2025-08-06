using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    // Speed at which the player moves
    private float playerSpeed;
    public float walkSpeed = 6f;
    public float sprintSpeed = 8.5f;
    // Reference to the CharacterController component
    private CharacterController myCC;
    private StaminabarController staminabarController;
    // Stores raw input values from keyboard/controller
    private Vector3 inputVector;

    // Final movement vector applied to the player
    private Vector3 movementVector;

    // Custom gravity value applied to the player
    private float myGravity = -10f;

    private bool lockMovement = false;
    private Animator handAnimator;
    private bool isWalking = true;

    public float normalFOV = 60f;
    public float sprintFOV = 70f;
    public float fovTransitionSpeed = 5f;
    public Rigidbody rb;
    private Camera playerCamera;
    public bool ignoreHitStop = false;
    [Header("Dash Parameters")]
    // isDashing ← false
    // canDash ← true
    // dashDuration ← small value (e.g., 0.2 seconds)
    // dashCooldown ← longer value (e.g., 1 second)
    // dashDistance ← how far you want to dash
    // inputDirection ← direction from player input (normalized)
    private bool isDashing = false;
    private bool canDash = true;
    [SerializeField]private float dashDuration = 0.2f;
    [SerializeField]private float dashCooldown = 1;
    [SerializeField]private float dashDistance = 2f;

    private bool sprintToggled = false;
    private float sprintToggleCooldown = 0.18f;
    private float lastSprintToggleTime = -1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Get and store the CharacterController component attached to the player GameObject
        myCC = GetComponent<CharacterController>();
        handAnimator = GetComponentInChildren<Animator>();
        staminabarController = FindObjectOfType<StaminabarController>();
        playerCamera = gameObject.GetComponentInChildren<Camera>();


    }

    // Update is called once per frame
    void Update()
    {

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        //shift holding still get sprint
        //if button is pressed and last frame, the button was pressed, its
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.JoystickButton8);
        bool isMoving = Mathf.Abs(inputX) > 0 || Mathf.Abs(inputZ) > 0;

        //  uncomment for sprint toglle toggle sprint
        // if (Input.GetKeyDown(KeyCode.JoystickButton8) && Time.time - lastSprintToggleTime > sprintToggleCooldown)
        // {
        //     sprintToggled = !sprintToggled;
        //     lastSprintToggleTime = Time.time;
        // }


        // If you previously used RT axis for sprint hold, we've removed that here.
        // sprintActive is true if player is using shift hold OR toggled on
        bool sprintActive = (shiftHeld || sprintToggled) && isMoving && staminabarController.playerStamina > 0f;

        // If stamina emptied, force toggle off
        if (staminabarController.playerStamina <= 0f)
        {
            sprintToggled = false;
            sprintActive = false;
        }
        if (isDashing == false)
        {

            //camera
            float targetFOV;

            if (sprintActive)
            {
                targetFOV = sprintFOV;
                isWalking = false;
                staminabarController.isSprinting = true;
                staminabarController.Sprinting();
            }
            else
            {
                targetFOV = normalFOV;
                isWalking = true;
                staminabarController.isSprinting = false;
            }

            //lerp to target fov
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovTransitionSpeed * Time.unscaledDeltaTime);

            playerSpeed = isWalking ? walkSpeed : sprintSpeed;
            // handle player input left right up down
            // get direction for player input
            GetInput(inputX, inputZ);
            if (lockMovement)
            {
                StopMovement();
                return;
            }

            // Move the player based on calculated movement vector
            if (ignoreHitStop)
            {
                MovePlayer(Time.unscaledDeltaTime);
            }
            else
            {
                MovePlayer(Time.deltaTime);
            }
        }
        if ((Input.GetKeyDown(KeyCode.Space) ||  Input.GetAxis("Controller LT") > 0.1f) && canDash && isMoving)
        {
            //start dash in input direction
            staminabarController.StaminaDash(inputVector);
        }
        else if ((Input.GetKeyDown(KeyCode.Space) ||  Input.GetAxis("Controller LT") > 0.1f) && canDash)
        {
            //start dash forward
            staminabarController.StaminaDash(transform.forward);
        }

    }
    public void StartDash(Vector3 inputDirection)
    {
        StartCoroutine(Dash(inputDirection));
    }

    IEnumerator Dash(Vector3 inputDirection) {
        // playerCamera.fieldOfView = sprintFOV;
        isDashing = true;
        canDash = false;
        float dashSpeed = dashDistance / dashDuration;
        float elapsed = 0;
        while (elapsed < dashDuration)
        {
            myCC.Move(inputDirection * dashSpeed * Time.unscaledDeltaTime);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        isDashing = false;
        // playerCamera.fieldOfView = normalFOV;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public void SetRunSpeed(float speed)
    {
        sprintSpeed = speed;
    }

    // Handles player input and calculates movement direction
    void GetInput(float inputx, float inputz)
    {
        // Get raw input from keyboard/controller for horizontal (A/D or left/right) and vertical (W/S or up/down)
        inputVector = new Vector3(inputx, 0f, inputz);
        // Normalize input vector to prevent faster diagonal movement
        inputVector.Normalize();

        
        if (inputVector.magnitude > 0)
        {
            handAnimator.SetBool("isWalking", true);
        }
        else
        {
            handAnimator.SetBool("isWalking", false);
        }
        // Convert local input direction to world space based on player's orientation
        inputVector = transform.TransformDirection(inputVector);

        // Combine movement with gravity to form the final movement vector
        movementVector = (inputVector * playerSpeed) + (Vector3.up * myGravity);
    }

    // Moves the player character using CharacterController
    void MovePlayer(float time)
    {
        myCC.Move(movementVector * time);


    }

    void StopMovement()
    {
        if (myCC.enabled)
        {
            movementVector = Vector3.zero;
            myCC.Move(movementVector); // ensures no residual movement
        }

    }

    public void LockMovement()
    {
        lockMovement = true;
    }

    public void UnlockMovement()
    {
        lockMovement = false;
    }
    public void ApplyKnockback(Vector3 force, float duration)
    {
        StartCoroutine(KnockbackRoutine(force, duration));
    }

    private IEnumerator KnockbackRoutine(Vector3 force, float duration)
    {
        LockMovement(); // stop CharacterController from overriding knockback
        rb.isKinematic = false; // enable physics for knockback 
        myCC.enabled = false;
        float timer = 0f;
        Vector3 initialForce = force;

        while (timer < duration)
        {
            // Apply diminishing force
            float dampFactor = 1f - (timer / duration); // goes from 1 to 0
            rb.velocity = initialForce * dampFactor;

            timer += Time.deltaTime;
            yield return null;
        }
        rb.isKinematic = true; // disable physics again
        myCC.enabled = true;
        UnlockMovement();
    }

    public void Move(float speed, Vector3 direction)
    {
        myCC.Move(speed * direction * Time.unscaledDeltaTime);
    }
}
