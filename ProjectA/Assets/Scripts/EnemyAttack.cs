using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float attackRange;
    private float distance;
    private Animator enemyAnimator;
    private BoxCollider attackCollider;
    private Enemy enemy;

    void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        enemyAnimator = GetComponentInChildren<Animator>();
        attackCollider = GetComponentInChildren<BoxCollider>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= attackRange)
        {
            enemy.LockMovement();
            Attack();
        }
    }
    //function to start attack
    //function to 
    public void Attack()
    {
        enemyAnimator.SetTrigger("isAttacking");

    }


    public void EnableCollider()
    {
        attackCollider.enabled = true;
    }
    
    public void DisableCollider()
    {
        attackCollider.enabled = false;
    }

}
