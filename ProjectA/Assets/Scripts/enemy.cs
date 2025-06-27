using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    //things that can change from other scripts
    public float stunDuration = 3f;
    public float moveSpeed = 2f;
    public bool isStunned = false;

    //things that prob shouldnt change from other scripts
    [SerializeField]
    private float wanderRadius,waitTime;

    [SerializeField]
    private GameObject stunEffect;

    [SerializeField]
    private Material stunMat;

    [SerializeField]
    private Material chillMat;

    private Coroutine stunCoroutine;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float waitTimer;
    
    
    private EnemyAwareness enemyAwareness;
    private Transform playertransform;
    private UnityEngine.AI.NavMeshAgent enemyNavMeshAgent;
    
    void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        playertransform = FindObjectOfType<PlayerMovement>().transform;
        enemyNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        chillMat = GetComponent<MeshRenderer>().material;
        startPosition = transform.position;
        PickNewDestination();
    }

    void Update()
    {
        //if aggro and not stunned follow
        if(enemyAwareness.isAggro && !isStunned){
            enemyNavMeshAgent.SetDestination(playertransform.position);
        }//else, just wander
        else if (enemyAwareness.isAggro == false && !isStunned){
            Wander();
        } 

    }

    public void Wander(){
        GetComponent<MeshRenderer>().material = chillMat;
        //move if not stunned
        if (!isStunned) {
            enemyNavMeshAgent.SetDestination(targetPosition);
        }
        //actual wandering
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f){
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime){
                PickNewDestination();
                waitTimer = 0f;
            }
        }
    }

    public void Stun()
    {
        //change material to stun
        GetComponent<MeshRenderer>().material = stunMat;
        //stun effect
        Instantiate(stunEffect, transform.position, Quaternion.identity);
        isStunned = true;  
        
        //stunlock player
        stunCoroutine = StartCoroutine(StunEnemy(stunDuration));
    }

    //execute stun for duration before making normal
    IEnumerator StunEnemy(float duration){
        enemyNavMeshAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    
    void PickNewDestination(){
        // Pick a new point within a small circle around the starting position
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        targetPosition = startPosition + new Vector3(randomPoint.x, 0, randomPoint.y);
    }
    
    public bool IsStunned()
    {
        return isStunned;
    }
    
}
