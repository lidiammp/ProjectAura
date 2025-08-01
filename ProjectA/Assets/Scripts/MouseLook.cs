using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float lookSpeed = 2f;
    public Transform playerCamera;
    private bool lockMouse = false;
    private float pitch = 0;
    private float yaw = 0;

    [SerializeField] private float controllerLookSpeed = 90f;
    [SerializeField] private float lookXLimit = 45f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (characterController.enabled)
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }

        if (!lockMouse)
        {
            pitch += -Input.GetAxis("Mouse Y") * lookSpeed;
            yaw = Input.GetAxis("Mouse X") * lookSpeed;
            //limit rotation so u dont break ur neck
            

            yaw += Input.GetAxis("Controller Yaw") * controllerLookSpeed * Time.deltaTime; // deg/sec * time = deg
            pitch +=  Input.GetAxis("Controller Pitch") * controllerLookSpeed * Time.deltaTime;



            pitch = Mathf.Clamp(pitch, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

            //rotate player left and right based on mouse position
            transform.rotation *= Quaternion.Euler(0, yaw, 0);
        }

        
        // controller left right value
        // scale it 
        // add to rotation

        //example 90 per second = certain number of second
        //look accelratio nis kinda cool

    }

    public void LockMouse()
    {
        lockMouse = true;
    }

    public void UnlockMouse()
    {
        lockMouse = false;
    }

    public void RotateToPoint(Transform point)
    {
        Vector3 direction = point.position - transform.position;
        direction.y = 0; // Keep only the horizontal direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}

