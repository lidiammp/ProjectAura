using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float attackRange;
    private float distance;
    private Animator enemyAnimator;
    private BoxCollider attackCollider;
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        enemyAnimator = GetComponent<Animator>();
        attackCollider = GetComponentInChildren<BoxCollider>();
        
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= attackRange)
        {
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
