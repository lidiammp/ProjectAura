using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRadius = 15f;
    public bool isAggro;
    // public Material aggroMat;
    private Enemy enemy;
    private Transform playertransform;
    private Animator enemyAnimator;
    void Start(){
        enemyAnimator = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();
        playertransform = FindObjectOfType<PlayerMovement>().transform;
    }
    void Update(){
        var dist = Vector3.Distance(playertransform.position, transform.position);
        if (dist <= awarenessRadius)
        {
            isAggro = true;
        }
        else if (dist > awarenessRadius)
        {
            isAggro = false;
            enemyAnimator.SetBool("isAggro", isAggro);
            
        }
        
        if (isAggro && enemy.GetIsStunned() == false)
        {
            enemyAnimator.SetBool("isAggro", isAggro);
            // GetComponent<MeshRenderer>().material = aggroMat;
            //set aggro
        }
    }    

}
