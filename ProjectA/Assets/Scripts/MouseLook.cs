using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1.5f;
    public float smoothing = 1.5f;

    public Transform playerCamera;

    private float xMousePos;
    private float yMousePos;

    private float smoothedX;
    private float smoothedY;

    private float yaw;
    private float pitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInput();
        ModifyInput();
        MovePlayer();
    }

    void GetInput()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
    }

    void ModifyInput()
    {
        xMousePos *= sensitivity * smoothing;
        yMousePos *= sensitivity;

        smoothedX = Mathf.Lerp(smoothedX, xMousePos, Time.deltaTime * smoothing);
        smoothedY = Mathf.Lerp(smoothedY, yMousePos, Time.deltaTime);
    }

    void MovePlayer()
    {
        // Horizontal rotation (yaw) on Player
        yaw += smoothedX;
        transform.localRotation = Quaternion.Euler(0f, yaw, 0f);

        // Vertical rotation (pitch) on Camera
        pitch -= smoothedY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}

