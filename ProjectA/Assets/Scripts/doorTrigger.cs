using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public GameObject Door;
    private MeshRenderer buttonRenderer;
    MeshRenderer doorRenderer;
    private BoxCollider doorCollider;
    private bool playerInRange = false;

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
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && Door != null)
        {
            Door.GetComponent<Animator>().Play("Open");
            buttonRenderer.material.color = Color.green;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("here");
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("not here");
            
        }
    }
}


