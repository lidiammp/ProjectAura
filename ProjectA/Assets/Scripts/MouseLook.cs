using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{ 
// {
//     public float sensitivity = 0.5f;
//     public float smoothing = 0.5f;

//     public Transform playerCamera;

//     private float xMousePos;
//     private float yMousePos;

//     private float smoothedX;
//     private float smoothedY;

//     private float yaw;
//     private float pitch;

//     private void Start()
//     {
//         // Remove cursor visibility 
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//     }

//     void Update()
//     {
//         GetInput();
//         ModifyInput();
//         MovePlayer();
//     }

//     void GetInput()
//     {
//         xMousePos = Input.GetAxisRaw("Mouse X");
//         yMousePos = Input.GetAxisRaw("Mouse Y");
//         // xMousePos *= sensitivity * Time.deltaTime;
//         // yMousePos *= sensitivity * Time.deltaTime;
//     }

//     void ModifyInput()
//     {
//         // xMousePos *= sensitivity * smoothing;
//         // yMousePos *= sensitivity * smoothing;

//         smoothedX = Mathf.Lerp(smoothedX, xMousePos, Time.deltaTime * smoothing * sensitivity);
//         smoothedY = Mathf.Lerp(smoothedY, yMousePos, Time.deltaTime * smoothing * sensitivity);
//         // xMousePos *= sensitivity * Time.deltaTime;
//         // yMousePos *= sensitivity * Time.deltaTime;
//     }

//     void MovePlayer()
//     {
//         // Horizontal rotation on Player
//         yaw += smoothedX;
//         transform.localRotation = Quaternion.Euler(0f, yaw, 0f);

//         // Vertical rotation on Camera
//         pitch -= smoothedY;
//         pitch = Mathf.Clamp(pitch, -89f, 89f);
//         playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
//     }

public Camera playerCamera;

    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Update()
    {
        if (cameraCanMove)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
            {
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }
    }
}

