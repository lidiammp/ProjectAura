using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float lookSpeed = 2f;
    public Transform playerCamera;
    private bool lockMouse = false;
    private float rotationX = 0;
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
        characterController.Move(moveDirection * Time.deltaTime);
        if (!lockMouse)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            //limit rotation so u dont break ur neck
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            //rotate player left and right based on mouse position
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }


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

