using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTooltipTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private TooltipManager tooltipManager;
    private bool hasEntered = false;
    void Start()
    {
        tooltipManager = FindObjectOfType<TooltipManager>();
    }
    // once it goes into the box, have a way to toggle 
    //only once it has entered, eneble the dash thing
    // once toggled on, check if has dashed
    // if dashed destroy the trigger and disable the ui
    void Update()
    {
        if (hasEntered)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                tooltipManager.DisableDashTooltip();
                Destroy(gameObject);
            }

        }
    }
    void OnTriggerEnter(Collider other)
    {
        hasEntered = true;
        if (other.GetComponent<PlayerMovement>())
        {
            tooltipManager.EnableDashTooltip();
        }
    }
    
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.GetComponent<PlayerMovement>())
    //     {
    //         tooltipManager.DisableDashTooltip();
    //     }
    // }
}
