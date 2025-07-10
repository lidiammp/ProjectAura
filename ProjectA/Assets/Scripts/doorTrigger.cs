using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public GameObject Door;
    private MeshRenderer buttonRenderer;
    MeshRenderer doorRenderer;
    private BoxCollider doorCollider;

    void Start()
    {
        doorRenderer = Door.GetComponent<MeshRenderer>();
        doorCollider = Door.GetComponent<BoxCollider>();
        buttonRenderer = GetComponent<MeshRenderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = Color.red;
        }
    }
    // void OnMouseDown()
    void Update()
    {
        //ADD LOGIC
        //only open door when Beam Collider is on Button and pressing E P-----------O
        //                                                              |          \|/
        //                                                                          ^
        if (Input.GetKeyDown(KeyCode.E) && Door != null)
        {
            Door.GetComponent<Animator>().Play("Open");
            buttonRenderer.material.color = Color.green;
        }

    }
}


