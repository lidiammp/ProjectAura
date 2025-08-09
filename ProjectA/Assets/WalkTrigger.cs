using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkTrigger : MonoBehaviour
{
    [SerializeField] private DoorController door;
    [SerializeField] private Room room;
    private bool hasActivated = false;
    void OnTriggerEnter(Collider other)
    
    {
        if (hasActivated) return;
        if (other.gameObject.CompareTag("Player"))
        {
            room.StartRoom();
        }
    }
}
