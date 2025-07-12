using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage;
    Rigidbody rb;
    [SerializeField] private float speed;
    Animator bulletAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        bulletAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = target.position - gameObject.transform.position;
        rb.AddForce(direction * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        bulletAnimator.SetBool("isCollided", true);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Healthbar>().TakeDamage(damage);
        }

        //Destroy(gameObject);
    }
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
