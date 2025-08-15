using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
    // [SerializeField] private float lookAcceleration;
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

            //raw input
            Vector2 rawInputStick = new Vector2(Input.GetAxis("Controller Yaw"), Input.GetAxis("Controller Pitch"));
            //deadzone magnitude of vector
            Vector2 stick = ApplyDeadzone(rawInputStick, 0.25f);

            float stickMag = stick.magnitude;
            Vector2 stickDir = stick.normalized;
            float nonLinearStickMagnitude = NonLinearInput(stickMag);
            Vector2 nonLinearStick = stickDir * nonLinearStickMagnitude;

            yaw += nonLinearStick.x * controllerLookSpeed * Time.deltaTime; // deg/sec * time = deg
            pitch += nonLinearStick.y * controllerLookSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

            //rotate player left and right based on mouse position
            transform.rotation *= Quaternion.Euler(0, yaw, 0);
        }


        //button check

        
        // controller left right value
        // scale it 
        // add to rotation

        //example 90 per second = certain number of second
        //look accelratio nis kinda cool

    }

    Vector2 ApplyDeadzone(Vector2 stick, float deadzone) {
        //go from 0 -1
        //rescale 
        //radial deadzone instead of boxy
        float magnitude = stick.magnitude;
        Vector2 direction = stick.normalized;
        //math on mag
        //scale dir by new deadzone mag
        float deadzonedMagnitude = Mathf.InverseLerp(deadzone, 1, magnitude);
         
        //inverse lerp -> 2 values gets inbetween num 

        return direction * deadzonedMagnitude;
    }
    float NonLinearInput(float x)
    {
        float SquareInput(float x)
        {
            return Mathf.Abs(x) * x;
            //keep the sign
        }

        float SmoothStepInput(float x)
        {
            //make sure its in 0 - 1
            //make positive and then clamp
            float t = Mathf.Clamp01(Mathf.Abs(x));
            //smoothStep func
            //keep sign
            return Mathf.Sign(x) * Mathf.SmoothStep(0, 1, t);
        }

        
        // square input and then
        // smoothstep squared val
        float CubeInput(float x)
        {
            return x * x * x;
        }

        float ApplyCurve(float x, float steepness)
        {
            //log till 0.5 and ehen curve up
            if (x <= 0.5f)
            {
                // Log-like slow start (scaled to fit 0–0.5 range)
                float t = x / 0.5f; // normalize 0–0.5 → 0–1
                return (Mathf.Log(1 + steepness * t) / Mathf.Log(1 + steepness)) * 0.5f;
            }
            else
            {
                // Exponential finish (scaled to fit 0.5–1 range)
                float t = (x - 0.5f) / 0.5f; // normalize 0.5–1 → 0–1
                return x;
            }
        }
        return SquareInput(x);
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

