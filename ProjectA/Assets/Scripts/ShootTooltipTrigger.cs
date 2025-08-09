using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTooltipTrigger : MonoBehaviour
{
    
    // Start is called before the first frame update
    private TooltipManager tooltipManager;
    void Start()
    {
        tooltipManager = FindObjectOfType<TooltipManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            tooltipManager.EnableShootTooltip();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            tooltipManager.DisableShootTooltip();
        }
    }
}
