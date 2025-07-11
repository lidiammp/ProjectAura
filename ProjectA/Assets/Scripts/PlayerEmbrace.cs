using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

public class PlayerEmbrace : MonoBehaviour
{
    [SerializeField] private float healValue;
    public float embraceRange = 2f;
    public float chargeTime = 1f;
    private float holdTimer = 0f;
    private bool isCharging = false;
    // public LayerMask enemyLayer;
    private EnemyManager enemyManager;
    private Healthbar playerHealth;
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Healthbar>();
    }

    // Start is called before the first frame update
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            isCharging = true;
            holdTimer = 0f;
            Debug.Log("warming up hands for hug");
        }


        if (Input.GetKey(KeyCode.E) && isCharging)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= chargeTime)
            {
                Debug.Log("bring it in brother");
                Debug.Log($" Embrace charge complete: held for {holdTimer:F2} seconds (needed {chargeTime} seconds).");

                TryEmbrace();

                // reset
                holdTimer = 0f;
                isCharging = false;
            }
        }

        // Cancel charge if player releases early
        if (Input.GetKeyUp(KeyCode.E) && isCharging)
        {
            Debug.Log("Im not warmed up enough");
            holdTimer = 0f;
            isCharging = false;
        }

    }
    void TryEmbrace()
    {
        // Collider[] hits= Physics.OverlapSphere(transform.position, embraceRange, enemyLayer);
        // foreach (var hit in hits)

        Collider[] hits = Physics.OverlapSphere(transform.position, embraceRange);
        foreach (var hit in hits)


        {
            Enemy enemyComponent = hit.GetComponent<Enemy>();
            if (enemyComponent != null && enemyComponent.GetIsStunned())
            {
                EmbraceEnemy(enemyComponent);
                return; // laddies leave me alone type shift, one at a time 
            }
        }
        void EmbraceEnemy(Enemy target)
        {
            // for now we js destroy it or maybe zion can add animation 
            // (pls someone teach me I have ptsd from when i touched animation and deleted everythingggg)

            //remove from list
            enemyManager.RemoveEnemy(target);
            target.GetComponent<Animator>().Play("MunchkinHug");
            //25% chance to heal by healvalue
            //might change to a different value based on type of enemy
            if (Random.value <= 0.25)
            {
                playerHealth.Heal(healValue);
            }

            //target.Die();
            Debug.Log("HUG!!!");
        }
    }
    
    public float GetHeldTime()
    {
        return holdTimer;
    }

    public bool IsCharging()
    {
        return isCharging;
    }
}
