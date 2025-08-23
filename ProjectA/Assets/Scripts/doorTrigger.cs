using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public DoorController[] doors;
    public GameObject[] trapDoors;
    private MeshRenderer buttonRenderer;

    public float interactRange = 5f; // max distance allowed for interaction
    public Transform player;        // assign Player transform in inspector

    public bool playerLook = false;
    public GameObject UIpopup;

    private bool isOn = false;
    void Start()
    {
        buttonRenderer = GetComponent<MeshRenderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = Color.red;
        }
        UIpopup?.SetActive(false);
        //uianimator = UIpopup.GetComponent<Animator>(); when adding  animation to door 
        foreach (DoorController door in doors)
        {
            if (door != null)
            {

                door.SetOpen(isOn);
            }
            else
            {
                Debug.LogError("A door reference is missing in " + gameObject.name);
            }
        }
    }
    // void OnMouseDown()
    void Update()
    {
        //ADD LOGIC
        //only open door when Beam Collider is on Button and pressing E P-----------O
        //         
        // check if player is looking and the button is on                                                     |          \|/
        if (playerLook && !isOn)
        {
            //get distance from player 
            float dist = Vector3.Distance(player.position, transform.position);
            //check if player is in range and that the player has clicked e
            if (dist <= interactRange)
            {
                UIpopup?.SetActive(true);
            }
            if (dist <= interactRange && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)))
            {
                //if thats the case activate the button and set the doors off
                ActivateButton();
            }
            
        } 


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BEAM"))
        {
            playerLook = true;
            // Debug.Log("here");

            


            //uianimator.enabled = true; when adding  animation to door 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BEAM"))
        {
            playerLook = false;
            // Debug.Log("not here");
            UIpopup?.SetActive(false);
            //uianimator.enabled = false;  when adding  animation to door 

        }
    }

    void ActivateButton()
    {
        isOn = true;
        //set the doors open
        foreach (DoorController door in doors)
        {
            door?.SetOpen(true);

        }

        // buttonRenderer.material.color = Color.green;

        //set the trap doors off
        foreach (GameObject trapDoor in trapDoors)
        {
            trapDoor?.SetActive(false);
        }
        
        //set the button off and turn off the ui prompt
        gameObject.SetActive(false);
        UIpopup?.SetActive(false);
    }
}


