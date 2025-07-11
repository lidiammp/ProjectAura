using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //player takes damage
            other.GetComponent<Healthbar>().TakeDamage(damage);
            //disable collider once it takes damage
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
