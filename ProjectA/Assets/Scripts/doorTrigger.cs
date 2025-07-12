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
    public GameObject UIpopup;
    private Animator uianimator;

    void Start()
    {
        doorRenderer = Door.GetComponent<MeshRenderer>();
        doorCollider = Door.GetComponent<BoxCollider>();
        buttonRenderer = GetComponent<MeshRenderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = Color.red;
        }
        UIpopup.SetActive(false);
        //uianimator = UIpopup.GetComponent<Animator>(); when adding  animation to door 

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
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("here");
            UIpopup.SetActive(true);
            //uianimator.enabled = true; when adding  animation to door 
        }
    }

    void OnCollisionExit (Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("not here");
            UIpopup.SetActive(false);
            //uianimator.enabled = false;  when adding  animation to door 

        }
    }
}


