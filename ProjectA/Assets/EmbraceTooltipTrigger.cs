using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmbraceTooltipTrigger : MonoBehaviour
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
            tooltipManager.EnableEmbraceTooltip();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            tooltipManager.DisableEmbraceTooltip();
        }
    }
}
