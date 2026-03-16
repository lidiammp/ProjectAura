using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmbraceTooltipTrigger : MonoBehaviour
{
    
    // Start is called before the first frame update
    private TooltipManager tooltipManager;
    [SerializeField] private Enemy enemy;
    void Start()
    {
        tooltipManager = FindObjectOfType<TooltipManager>();
    }
    void Update()
    {
        if (enemy.IsDestroyed())
        {
            tooltipManager.DisableEmbraceTooltip();
            Destroy(gameObject);
        }
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
