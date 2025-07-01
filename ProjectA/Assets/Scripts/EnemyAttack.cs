using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }
    
    //function to start attack
    //function to 
}
