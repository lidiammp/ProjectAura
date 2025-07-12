using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{


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
    void Start()
    {
        //if no centerpoint use enemys own position
        if (centrePoint == null)
        {
            centrePoint = transform;
        }
        //add player to obstacle checkrer
        layersToHit |= 1 << LayerMask.NameToLayer("Player");
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyAwareness = GetComponent<EnemyAwareness>();
        playertransform = FindObjectOfType<PlayerMovement>().transform;
        enemyNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        maxDistance = enemyAwareness.awarenessRadius;
    }

    void Update()
    {
        //if aggro and not stunned follow player 
        if (enemyAwareness.isAggro && isStunned == false && CheckForObstacle())
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
    public void Wander()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            waitTimer = waitTime; // Start waiting
        }
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f)
        {
            if (enemyNavMeshAgent.remainingDistance <= enemyNavMeshAgent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, wanderRadius, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    enemyNavMeshAgent.SetDestination(point);
                }
            }
        }
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



    //execute stun for duration before unlocking player
    IEnumerator StunEnemy(float duration)
    {
        enemyNavMeshAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(duration);
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
        enemyNavMeshAgent.ResetPath();
    }

    public void DisableBodyCollider()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }
}
