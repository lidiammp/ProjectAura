using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // [SerializeField] private doorTrigger button;
    private Animator doorAnimator;
    [SerializeField] private bool openDefault;
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
        if (openDefault)
        {
            SetOpen(openDefault);
        }
    }
    
    //set open or closed based on parameter
    public void SetOpen(bool isOpen)
    {
        doorAnimator?.SetBool("isOpen", isOpen);
    }
}
