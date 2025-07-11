using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{

    // charge settings

    public float chargeTimeRequired = 2f;
    private float chargeHeldTime;
    private bool isCharging;


    // Reference to the trigger collider representing the beams range
    private BoxCollider beamTrigger;
    public float beamShotRadius = 20f;

    // Beam range forward and upward
    public float range = 20f;
    public float verticalRange = 20f;

    // How often the beam can be fired
    public float fireRate;

    // Tracks when the player can next fire
    private float nextTimeToFire;

    // Used to filter what the raycast can hit 
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;
    // Reference to EnemyManager which tracks enemies in range
    private EnemyManager enemyManager;

    private AudioSource audioSource;
    private Animator handAnimator;
    private GameObject parent;
    void Start()
    {
        //beam sound
        audioSource = GetComponent<AudioSource>();
        enemyManager = FindObjectOfType<EnemyManager>();
        // Get the BoxCollider and configure its size/position to match beam range
        beamTrigger = GetComponent<BoxCollider>();
        beamTrigger.size = new Vector3(1, verticalRange, range);
        beamTrigger.center = new Vector3(0, 0, range * 0.5f);
        parent = transform.parent.gameObject;
        handAnimator = parent.GetComponentInChildren<Animator>();

    }

    void Update()
    {
        ChargeBeam();
        if (Input.GetKeyUp(KeyCode.Mouse0) && isCharging)
        {
            ShootBeam();
        }
    }
    void ChargeBeam()
    {
        //charge
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isCharging = true;
            chargeHeldTime = 0f; // reset when charging starts
            handAnimator.SetBool("isCharging", true);
            // lidia wants a sound here
        }

        //while its chargin
        if (isCharging && Input.GetKey(KeyCode.Mouse0))
        {
            chargeHeldTime += Time.deltaTime;
            //accumulate charge time
        }

    }
    void ShootBeam()
    {
        // once its let go 
        isCharging = false;
        handAnimator.SetBool("isCharging", false);

        //check if its been long enough
        if (chargeHeldTime >= chargeTimeRequired)
        {
            handAnimator.SetTrigger("isAttacking");
            Fire();
        }
        //if hasnt show in debug
        else
        {
            Debug.Log("Charge not long enough!");
            // Optional: Play failed charge sound
        }

        chargeHeldTime = 0f; // Reset after release

    }

    void Fire()
    {
        //draw sphere for debuggin
        audioSource.Stop();
        audioSource.Play();

        //beamshot radius
        Collider[] enemyColliders;
        //each enemy in the area of overlap sphere becomes aggro.
        enemyColliders = Physics.OverlapSphere(transform.position, beamShotRadius, enemyLayerMask);

        foreach (var enemyCollider in enemyColliders)
        {
            enemyCollider.GetComponent<EnemyAwareness>().isAggro = true;
        }


        // Loop through all enemies currently in beam range
        foreach (var enemy in enemyManager.enemiesInTrigger)
        {
            // Calculate direction from beam to enemy
            var dir = enemy.transform.position - transform.position;
            RaycastHit hit;

            // Shoot a ray toward enemy, check if it's actually visible (no obstacles)
            if (Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
            {
                if (hit.transform == enemy.transform)
                {
                    // Confirmed hit - draw a debug ray and pause game (for testing)
                    //Debug.DrawRay(transform.position, dir, Color.green);
                    enemy.Stun(); // stun or in this case FREEZE
                }
            }
        }

        // Reset the fire cooldown timer
        nextTimeToFire = Time.time + fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When something enters beam range, check if it's an enemy
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy)
        {
            // Add enemy to the manager's tracking list
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When something exits beam range, check if it's an enemy
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy)
        {
            // Remove enemy from tracking list
            enemyManager.RemoveEnemy(enemy);
        }
    }

    public float GetHeldTime()
    {
        return chargeHeldTime;
    }

    public bool IsCharging()
    {
        return isCharging;
    }
    
    
}

