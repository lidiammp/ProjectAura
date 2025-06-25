using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    // Reference to the trigger collider representing the beam's range
    private BoxCollider beamTrigger;

    // Beam range forward and upward
    public float range = 20f;
    public float verticalRange = 20f;

    // How often the beam can be fired
    public float fireRate;

    // Tracks when the player can next fire
    private float nextTimeToFire;

    // Used to filter what the raycast can hit 
    public LayerMask raycastLayerMask;

    // Reference to EnemyManager which tracks enemies in range
    public EnemyManager EnemyManager;

    void Start()
    {
        // Get the BoxCollider and configure its size/position to match beam range
        beamTrigger = GetComponent<BoxCollider>();
        beamTrigger.size = new Vector3(1, verticalRange, range);
        beamTrigger.center = new Vector3(0, 0, range * 0.5f);
    }

    void Update()
    {
        // Press e to fire the beam if cooldown is over
        if (Input.GetKeyDown(KeyCode.E) && Time.time > nextTimeToFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        // Loop through all enemies currently in beam range
        foreach (var enemy in EnemyManager.enemiesInTrigger)
        {
            // Calculate direction from beam to enemy
            var dir = enemy.transform.position - transform.position;
            RaycastHit hit;

            // Shoot a ray toward enemy, check if it's actually visible (no obstacles)
            if (Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
            {
                if (hit.transform == enemy.transform)
                {
                    // Confirmed hit — draw a debug ray and pause game (for testing)
                    Debug.DrawRay(transform.position, dir, Color.green);
                    enemy.Stun(); //freeze da enemy
                  
                }
            }
        }

        // Reset the fire cooldown timer
        nextTimeToFire = Time.time + fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When something enters beam range, check if it's an enemy
        enemy enemy = other.transform.GetComponent<enemy>();
        if (enemy)
        {
            // Add enemy to the manager's tracking list
            EnemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When something exits beam range, check if it's an enemy
        enemy enemy = other.transform.GetComponent<enemy>();
        if (enemy)
        {
            // Remove enemy from tracking list
            EnemyManager.RemoveEnemy(enemy);
        }
    }
}

