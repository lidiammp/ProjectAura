using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth;

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

    [SerializeField] private Transform centrePoint;
    private Animator enemyAnimator;
    private EnemyAwareness enemyAwareness;
    private Transform playertransform;
    private UnityEngine.AI.NavMeshAgent enemyNavMeshAgent;
    public float waitTime = 2f;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private float maxDistance;
    public LayerMask layersToHit;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    [ColorUsage(true) ] public Color pinkColor = new Color(1f, 0.75f, 0.8f, 1f);
    private TimeManager timeManager;

    private Rigidbody enemyRigidBody;
    public float snapThreshold = 2f;
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        //if aggro and not stunned follow player 
        if (enemyAwareness.isAggro && isStunned == false && CheckForObstacle() && enemyNavMeshAgent.enabled && enemyNavMeshAgent.isOnNavMesh)
        {
            enemyNavMeshAgent.SetDestination(playertransform.position);
        }//else, just wander
        else if (enemyAwareness.isAggro == false && isStunned == false)
        {
            Wander();
        }

    }

    private bool CheckForObstacle()
    {
        Vector3 dir = (playertransform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layersToHit) && hit.collider.gameObject.tag == "Player")
        {
            //return hit only if the hit hits a playr
            return true;
            //Time.timeScale = 0;

        }
        //if it hits anything else just chill
        return false;

    }

    public void TakeDamage(int amount)
    {
        
    
        enemyAnimator.SetTrigger("isTakingDamage");
        
        
        StartCoroutine(FlashRed());
        timeManager.Stop(0.15f);
        ApplyKnockback(10*-transform.forward + Vector3.up, 0.3f);
        if (isStunned) return; // Can't take damage while stunned

        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            Stun();
            return;
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = pinkColor;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }
    public void Wander()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            waitTimer = waitTime; // Start waiting
        }
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f && enemyNavMeshAgent.enabled && enemyNavMeshAgent.isOnNavMesh)
        {
            if (enemyNavMeshAgent.remainingDistance <= enemyNavMeshAgent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, wanderRadius, out point)) //pass in our centre point and radius of area
                {
                    // Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    enemyNavMeshAgent.SetDestination(point);
                }
            }
        }
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


    public void Stun()
    {

        //show stun effect 
        Instantiate(stunEffect, transform.position, Quaternion.identity);
        //set variables
        isStunned = true;
        enemyAnimator.SetBool("isStunned", isStunned);
        //stunlock player
        StartCoroutine(StunEnemy(stunDuration));
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
        enemyAnimator.SetBool("isStunned", isStunned);

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
}
