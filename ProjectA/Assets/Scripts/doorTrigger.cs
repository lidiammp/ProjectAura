using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public DoorController[] doors;
    public GameObject[] trapDoors;
    private MeshRenderer buttonRenderer;

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
        //                                                              |          \|/
        //                                                                          ^
        if (playerLook && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)) && !isOn) // only open if not already open
        {
            isOn = true;
            foreach (DoorController door in doors)
            {
                if (door != null)
                {
                    door.SetOpen(true);
                }
            }

            // buttonRenderer.material.color = Color.green;

            if (trapDoors.Length > 0)
            {
                foreach (GameObject trapDoor in trapDoors)
                {
                    trapDoor.SetActive(false);
                }
            }

            gameObject.SetActive(false);
            UIpopup?.SetActive(false);
        }


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerLook = true;
            // Debug.Log("here");

            UIpopup?.SetActive(true);
            
            
            //uianimator.enabled = true; when adding  animation to door 
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerLook = false;
            // Debug.Log("not here");
            UIpopup?.SetActive(false);
            //uianimator.enabled = false;  when adding  animation to door 

        }
    }
}


