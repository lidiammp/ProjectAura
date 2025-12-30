using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapFloor : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    // Start is called before the first frame update

    
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(floor);
        }
    }
}
