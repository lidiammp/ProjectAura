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
    [SerializeField]private float minDashDistance = 1f;
    float targetFOV;
    [Header("Camera Parameters")]
    
    public float normalFOV = 60f;
    public float dashFOV = 65f;
    public float sprintFOV = 70f;
    public float fovTransitionSpeed = 5f;
    public float dashFovTransitionSpeed = 5f;
    private bool sprintToggled = false;

    private float sprintToggleCooldown = 0.18f;
    private float lastSprintToggleTime = -1f;
    public bool isMoving = false;

    [Header("Sound Parameters")]
    public float stepDistance = 1.6f; // tune this
    private float distanceSinceLastStep = 0f;
    private Vector3 lastPosition;
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
        if (GameManager.instance.CurrentState == GameState.Paused) 
        {
            StopMovement();
            return;
        }
        //keyboard inputs
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        //shift holding still get sprint
        //if button is pressed and last frame, the button was pressed, its
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        // || Input.GetKey(KeyCode.JoystickButton8);
        isMoving = Mathf.Abs(inputX) > 0 || Mathf.Abs(inputZ) > 0;
        //play foot sound based on last position

        //uncomment for sprint toglle toggle sprint
        if (Input.GetKeyDown(KeyCode.JoystickButton8) && Time.time - lastSprintToggleTime > sprintToggleCooldown)
        {
            sprintToggled = !sprintToggled;
            lastSprintToggleTime = Time.time;
        }

        bool sprintAllowed = !staminabarController.GetCoolDown() && staminabarController.playerStamina > 0f;
        // If you previously used RT axis for sprint hold, we've removed that here.
        // sprintActive is true if player is using shift hold OR toggled on
        bool sprintActive = (shiftHeld || sprintToggled) && isMoving && sprintAllowed;
        
        // If stamina emptied, force toggle off
        if (staminabarController.playerStamina <= 0f)
        {
            sprintToggled = false;
            sprintActive = false;
        }

        handAnimator.SetBool("isRunning", sprintActive);

        //camera and FOV stuff
        if (!isDashing)
        {
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


            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovTransitionSpeed * Time.unscaledDeltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton4) && canDash && isMoving)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, dashFOV, dashFovTransitionSpeed * Time.unscaledDeltaTime);
                //start dash in input direction
                staminabarController.StaminaDash(inputVector);
            }
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton4) && canDash)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, dashFOV, dashFovTransitionSpeed * Time.unscaledDeltaTime);
                //start dash forward
                staminabarController.StaminaDash(transform.forward);
            }

    }
    public void StartDash(Vector3 inputDirection, float distance)
    {
        StartCoroutine(Dash(inputDirection, distance));
    }

    IEnumerator Dash(Vector3 inputDirection, float distance) {
        // playerCamera.fieldOfView = sprintFOV;
        isDashing = true;
        canDash = false;
        float dashSpeed = distance / dashDuration;
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

    public void PlayHeelOnMove()
    {
        if(isMoving)
        {
            //accumalate distance
            distanceSinceLastStep += Vector3.Distance(transform.position,lastPosition);
            if(distanceSinceLastStep > stepDistance)
            {
                AudioManager.instance.PlaySFXRandom("HeelStep");
                //reset step counter/accumulator
                distanceSinceLastStep = 0;
            }
        }
        else
        {
            //not moving so reset counter
            distanceSinceLastStep = 0;
        }
        lastPosition = gameObject.transform.position;
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
    public void ResetSpeedState()
    {
        // Reset speed after embrace
        bool sprintAllowed = !staminabarController.GetCoolDown()
                        && staminabarController.playerStamina > 0f;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        bool sprintActive = (shiftHeld || sprintToggled) && isMoving && sprintAllowed;

        if (sprintActive)
        {
            isWalking = false;
            playerSpeed = sprintSpeed;
        }
        else
        {
            isWalking = true;
            playerSpeed = walkSpeed;
        }
    }
    public void LockMovement()
    {
        lockMovement = true;
    }

    public void UnlockMovement()
    {
        lockMovement = false;
        ResetSpeedState();
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

    public float GetDashDistance(){
        return dashDistance;
    }

    public float GetMinDashDistance(){
        return minDashDistance;
    }
}
