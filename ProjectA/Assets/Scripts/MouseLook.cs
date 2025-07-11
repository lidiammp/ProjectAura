using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float lookSpeed = 2f;
    public Transform playerCamera;

    private float rotationX = 0;
    [SerializeField] private float lookXLimit = 45f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        characterController.Move(moveDirection * Time.deltaTime);

        //rotate camera up and down based on mouse position
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        //limit rotation so u dont break ur neck
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        //rotate player left and right based on mouse position
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }



}

