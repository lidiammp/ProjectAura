using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public DoorController[] doors;
    private MeshRenderer buttonRenderer;
    MeshRenderer doorRenderer;
    private BoxCollider doorCollider;
    public bool playerLook = false;
    public GameObject UIpopup;
    private Animator uianimator;
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
            door.SetOpen(isOn);
        }
    }
    // void OnMouseDown()
    void Update()
    {
        //ADD LOGIC
        //only open door when Beam Collider is on Button and pressing E P-----------O
        //                                                              |          \|/
        //                                                                          ^
        if (playerLook && Input.GetKeyDown(KeyCode.E))
        {
            foreach (DoorController door in doors)
            {
                //toggle door thing
                isOn = !isOn;
                if (door != null){
                    door.SetOpen(isOn);
                }
                //change color
                if (isOn)
                    buttonRenderer.material.color = Color.green;
                else
                    buttonRenderer.material.color = Color.red;
            }
            

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


