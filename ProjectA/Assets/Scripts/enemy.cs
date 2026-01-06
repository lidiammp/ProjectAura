using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth;
    [SerializeField] float groupAlertRadius = 5f;
    public string enemyType;
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    //things that can change from other scripts
    public float stunDuration = 3f;
    public float moveSpeed = 2f;
    private bool isStunned = false;

    //things that prob shouldnt change from other scripts
    [SerializeField]
    private float wanderRadius;

    [SerializeField]
    private GameObject stunEffect;
    [SerializeField]
    private GameObject onhitEffect;
    [SerializeField] private Transform centrePoint;
    private Animator enemyAnimator;
    public AudioSource enemyAudioSource;
    private EnemyAwareness enemyAwareness;
    private Transform playertransform;
    private UnityEngine.AI.NavMeshAgent enemyNavMeshAgent;
    public float waitTime = 2f;
    // private float waitTimer = 0f;
    private bool isWaiting = false;
    private float maxDistance;
    public LayerMask layersToHit;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    [ColorUsage(true)] public Color pinkColor = new Color(1f, 0.75f, 0.8f, 1f);
    private TimeManager timeManager;

    private Rigidbody enemyRigidBody;
    public float snapThreshold = 2f;

    private Coroutine stunRoutine;
    OutlineManager outlineManager;
    [SerializeField] private bool isPermaStunned = false;
    void Start()
    {
        outlineManager = GetComponentInChildren<OutlineManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyAudioSource = GetComponent<AudioSource>();
        originalColor = spriteRenderer.color;
        currentHealth = maxHealth;
        //if no centerpoint use enemys own position
        if (centrePoint == null)
        {
            centrePoint = transform;
        }
        //add player to obstacle checkrer
        layersToHit |= 1 << LayerMask.NameToLayer("Player");
        enemyAnimator = GetComponentInChildren<Animator>();
        if (GetComponent<EnemyAwareness>())
        {
            enemyAwareness = GetComponent<EnemyAwareness>();
            maxDistance = enemyAwareness.awarenessRadius;
        }

        playertransform = FindObjectOfType<PlayerMovement>().transform;
        enemyNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        timeManager = FindObjectOfType<TimeManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool moving = enemyNavMeshAgent.enabled &&
                  enemyNavMeshAgent.isOnNavMesh &&
                  enemyNavMeshAgent.velocity.sqrMagnitude > 0.01f &&
                  !isStunned;

        if (!isPermaStunned)
        {
            enemyAnimator.SetBool("isWalking", moving);
            enemyAnimator.SetBool("isAggro", enemyAwareness.isAggro);
            enemyAnimator.SetBool("isStunned", isStunned);
        }


        //if aggro and not stunned follow player 
        if (enemyAwareness.isAggro && !isStunned && CheckForObstacle() && enemyNavMeshAgent.enabled && enemyNavMeshAgent.isOnNavMesh)
        {
            enemyNavMeshAgent.SetDestination(playertransform.position);
        }
        else if (!enemyAwareness.isAggro && !isStunned && !isPermaStunned)
        {
            Wander();
        }

    }

    private bool CheckForObstacle()
    {
        Vector3 enemyEyePos = transform.position + Vector3.up * 1.5f;        // Enemy eye height
        Vector3 playerEyePos = playertransform.position + Vector3.up * 1.0f; // Player eye height
        Vector3 direction = (playerEyePos - enemyEyePos).normalized;
        float distance = Vector3.Distance(enemyEyePos, playerEyePos);

        if (Physics.Raycast(enemyEyePos, direction, out RaycastHit hit, distance, layersToHit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true; // Player visible (line of sight)
            }
            else
            {
                return false; // Something blocking the view
            }
        }

        return false; // Nothing hit means no LOS
    }

    public void TakeDamage(int amount)
    {
        // show the particles
        Instantiate(onhitEffect, transform.position, Quaternion.identity);
        timeManager.Stop(0.15f);
        if (isPermaStunned) return;
        enemyAnimator.SetTrigger("isTakingDamage");
        AudioManager.instance.PlaySFX("EnemyTakeDamage");
        // StartCoroutine(FlashRed());
        ApplyKnockback(10 * -transform.forward + Vector3.up, 0.3f);
        // if (isStunned) return; // Can't take damage while stunned

        currentHealth -= amount;
        enemyAwareness.AlertNearby(groupAlertRadius);
        if (currentHealth <= 0)
        {
            Stun(stunDuration);
            return;
        }
        if (isStunned)
        {
            Stun(stunDuration);  // Re-trigger the stun effect and coroutine
            return;

        }




    }
    //doesnt work rn cuz the animator has control :(
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = pinkColor;
        yield return new WaitForSeconds(1f);
        spriteRenderer.color = originalColor;
    }
    public void Wander()
    {
        //if there enemy moving or not on surface or not enabled
        if (enemyNavMeshAgent.pathPending || !enemyNavMeshAgent.isOnNavMesh || !enemyNavMeshAgent.enabled)
            return;

        //if has a destination and has met it
        if (!enemyNavMeshAgent.hasPath || enemyNavMeshAgent.remainingDistance <= enemyNavMeshAgent.stoppingDistance)
        {
            // When not waiting, start a wait coroutine
            if (!isWaiting)
            {
                StartCoroutine(WanderPauseRoutine());
            }
            enemyAnimator.SetBool("isWalking", false);
        }
        else
        {

            enemyAnimator.SetBool("isWalking", true); // walk anim when moving
        }
    }


    private IEnumerator WanderPauseRoutine()
    {
        isWaiting = true;

        //wait at the current spot
        yield return new WaitForSeconds(waitTime);

        //pick a new destination after waiting
        Vector3 point;
        if (RandomPoint(centrePoint.position, wanderRadius, out point))
        {
            // Debug.Log("Wander point set: " + point);
            if (enemyNavMeshAgent.enabled && enemyNavMeshAgent.isOnNavMesh)
            {
                enemyNavMeshAgent.SetDestination(point);
            }
        }
        // else
        // {
        //     // Debug.LogWarning("Failed to find valid wander point");
        // }

        isWaiting = false;
    }

    public void ApplyKnockback(Vector3 force, float duration)
    {
        StartCoroutine(KnockbackRoutine(force, duration));
    }

    private IEnumerator KnockbackRoutine(Vector3 force, float duration)
    {
        // Disable NavMeshAgent
        enemyNavMeshAgent.enabled = false;

        // Enable physics
        enemyRigidBody.isKinematic = false;

        // Apply force
        enemyRigidBody.AddForce(force, ForceMode.Impulse);

        // Wait for knockback to play out
        yield return new WaitForSeconds(duration);

        // Stop movement
        enemyRigidBody.velocity = Vector3.zero;

        // Re-disable physics
        enemyRigidBody.isKinematic = true;

        // Re-enable NavMeshAgent
        enemyNavMeshAgent.enabled = true;
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    //out thing means that before it returns, it needs the result to not be null
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        //if not hit it wont return and will choose another random point
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop but thats lowkey tryhard
            result = hit.position;
            return true;
        }

        //if no random point try again
        result = Vector3.zero;
        return false;
    }


    public void Stun(float stunDuration)
    {
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
        }
        //show stun effect 
        // Instantiate(stunEffect, transform.position, Quaternion.identity);
        //set variables
        isStunned = true;

        enemyAnimator.SetBool("isStunned", isStunned);

        outlineManager.SetOutlineColor(new Color(255, 255, 255));
        //stunlock player
        stunRoutine = StartCoroutine(StunEnemy(stunDuration));

    }

    public void PermaStun()
    {

        // //show stun effect 
        // Instantiate(stunEffect, transform.position, Quaternion.identity);
        //set variables
        isStunned = true;
        // enemyAnimator.SetBool("isStunned", isStunned);
        //stunlock player
        StartCoroutine(StunEnemy(10000));
    }



    //execute stun for duration before unlocking player
    IEnumerator StunEnemy(float duration)
    {
        if (enemyNavMeshAgent.enabled)
        {
            enemyNavMeshAgent.SetDestination(transform.position);
        }

        yield return new WaitForSeconds(duration);
        currentHealth = maxHealth;
        isStunned = false;
        enemyAwareness.isAggro = false;
        enemyAnimator.SetBool("isStunned", isStunned);
        outlineManager.DisableOutline();

    }

    //method to get stun without changing
    public bool GetIsStunned()
    {
        return isStunned;
    }

    //method to invoke death for wave manager
    public void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    //method to lock movement on attack
    public void LockMovement()
    {
        if (enemyNavMeshAgent.enabled)
        {
            enemyNavMeshAgent.ResetPath();
        }

    }

    public void DisableBodyCollider()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
