using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Speed at which the player moves
    private float playerSpeed;
    public float walkSpeed = 20f;
    public float sprintSpeed = 30f;
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

    // Start is called before the first frame update
    void Start()
    {
        // Get and store the CharacterController component attached to the player GameObject
        myCC = GetComponent<CharacterController>();
        handAnimator = GetComponentInChildren<Animator>();
        staminabarController = FindObjectOfType<StaminabarController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isMoving = Mathf.Abs(inputX) > 0 || Mathf.Abs(inputZ) > 0;

        if (shiftHeld && isMoving && staminabarController.playerStamina > 0)
        {
            isWalking = false;
            staminabarController.isSprinting = true;
            staminabarController.Sprinting();
        }
        else
        {
            isWalking = true;
            staminabarController.isSprinting = false;
        }

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
        MovePlayer();
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
    void MovePlayer()
    {
        // Apply movement to the CharacterController over time
        myCC.Move(movementVector * Time.deltaTime);
    }

    void StopMovement()
    {
        movementVector = Vector3.zero;
        myCC.Move(movementVector); // ensures no residual movement
    }

    public void LockMovement()
    {
        lockMovement = true;
    }

    public void UnlockMovement()
    {
        lockMovement = false;
    }
}
