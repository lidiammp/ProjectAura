using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshEnabler : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    private bool hasLandedOnFloor = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnablePhysics();
        
    }
    public void EnablePhysics()
    {
        navMeshAgent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void DisablePhysics()
    {
        navMeshAgent.enabled = true;
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (!hasLandedOnFloor && collision.gameObject.CompareTag("Floor"))
        {
            hasLandedOnFloor = true;
            DisablePhysics(); //turn back enemy trackin
        }
    }
}
