using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private float fireRate;
    float fireTimer;
    private float distance;
    private Animator enemyAnimator;
    private BoxCollider attackCollider;
    private Enemy enemy;
    private bool canAttack;
    [SerializeField] private GameObject Bullet;
    void Start()
    {
        canAttack = true;
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        enemyAnimator = GetComponentInChildren<Animator>();
        attackCollider = GetComponentInChildren<BoxCollider>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        //if in attack range stop and attack
        //works for both ranged and melee
        fireTimer += Time.deltaTime;
        if (distance <= attackRange && fireTimer > fireRate && enemy.GetIsStunned() != true && canAttack == true)
        {

            enemy.LockMovement();
            Attack();
            fireTimer = 0;
        }
    }

    //function to start attack animation
    public void Attack()
    {
        enemyAnimator.SetTrigger("isAttacking");
        ShootBullet();
    }

    //                                     melee enemy                                               //
    //-----------------------------------------------------------------------------------------------//
    //method to enable collider in animation
    public void EnableCollider()
    {
        // attackCollider.enabled = true;
    }

    //method to disable collider in animation
    public void DisableCollider()
    {
        // attackCollider.enabled = false;
    }

    //                                     ranged enemy                                               //
    //-----------------------------------------------------------------------------------------------//
    public void ShootBullet()
    {
        if (Bullet != null)
        {
            Instantiate(Bullet, shootingPoint.transform.position, transform.rotation);
        }

    }

    public void DisableAttack()
    {
        canAttack = false;
    }
}
