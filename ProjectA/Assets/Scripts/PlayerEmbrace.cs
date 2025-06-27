using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmbrace : MonoBehaviour
{
    public float embraceRange = 2f;
    // public LayerMask enemyLayer;
    private EnemyManager enemyManager;
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    // Start is called before the first frame update
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryEmbrace();
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
            if (enemyComponent != null && enemyComponent.IsStunned())
            {
                EmbraceEnemy(enemyComponent);
                return; // laddies leave me alone type shift, one at a time 
            } 
        }
        void EmbraceEnemy(Enemy target)
        {
            // for now we js destroy it or maybe zion can add animation (pls someone teach me I have ptsd from when i touched animation and deleted everythingggg)
            enemyManager.RemoveEnemy(target);
            target.Die();
            Debug.Log("HUG!!!");
        }
    }

}
