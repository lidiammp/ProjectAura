using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Speed at which the player moves
    private float playerSpeed;
    public float walkSpeed = 20f;
    public float sprintSpeed = 30f;
    // Reference to the CharacterController component
    private CharacterController myCC;

    // Stores raw input values from keyboard/controller
    private Vector3 inputVector;

    // Final movement vector applied to the player
    private Vector3 movementVector;

    // Custom gravity value applied to the player
    private float myGravity = -10f;

    private bool lockMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get and store the CharacterController component attached to the player GameObject
        myCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = walkSpeed;
        if (Input.GetKey("left shift") || Input.GetKey("right shift"))
        {
            playerSpeed = sprintSpeed;
        }
        
        // Handle player input
        GetInput();
        if (lockMovement)
        {
            StopMovement();
            return;
        }
        
        // Move the player based on calculated movement vector
        MovePlayer();
    }

    // Handles player input and calculates movement direction
    void GetInput()
    {
        // Normalize input vector to prevent faster diagonal movement
        inputVector.Normalize();

        // Get raw input from keyboard/controller for horizontal (A/D or left/right) and vertical (W/S or up/down)
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

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
