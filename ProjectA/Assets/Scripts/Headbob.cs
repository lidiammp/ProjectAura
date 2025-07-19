using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    public CharacterController controller; // Assign your player controller
    public float bobFrequency = 1.5f;
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;
    private float timer = 0f;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForHeadbobTrigger();
    }
    private void StartHeadbob()
    {
        timer += Time.deltaTime * bobFrequency;
        float x = Mathf.Cos(timer) * bobHorizontalAmplitude;
        float y = Mathf.Sin(timer * 2) * bobVerticalAmplitude;

        transform.localPosition = startPosition + new Vector3(x, y, 0);
    }
    void CheckForHeadbobTrigger()
    {
        // float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        if (controller.velocity.magnitude > 0.1f)
        {
            StartHeadbob();
        }else
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startPosition,
                Time.deltaTime * 12f
            );
        }
    }


}
