using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
 public float moveSpeed = 2f;
    public float wanderRadius = 3f;
    public float waitTime = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float waitTimer;
    private bool isStunned = false;
    void Start()
    {
        startPosition = transform.position;
        PickNewDestination();
    }

    void Update()
    {
        if (isStunned) return;
        // Move toward the target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // reached the target pick a new one
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                PickNewDestination();
                waitTimer = 0f;
            }
        }
    }

public void Stun()
{
   isStunned = true;  
}


    void PickNewDestination()
    {
        // Pick a new point within a small circle around the starting position
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        targetPosition = startPosition + new Vector3(randomPoint.x, 0, randomPoint.y);
    }
    
    public bool IsStunned()
    {
        return isStunned;
    }
    
}
