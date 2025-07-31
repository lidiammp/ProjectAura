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
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isMoving = Mathf.Abs(inputX) > 0 || Mathf.Abs(inputZ) > 0;
        bool isMovingForward = inputZ > 0f;

        //camera
        float targetFOV;

        if (shiftHeld && isMoving && staminabarController.playerStamina > 0 && isMovingForward)
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

    public void SetRunSpeed(float speed)
    {
        sprintSpeed = speed;
    }

    // Handles player input and calculates movement direction
    void GetInput(float inputx, float inputz)
    {
        // Normalize input vector to prevent faster diagonal movement
        inputVector.Normalize();

        // Get raw input from keyboard/controller for horizontal (A/D or left/right) and vertical (W/S or up/down)
        inputVector = new Vector3(inputx, 0f, inputz);
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
}
