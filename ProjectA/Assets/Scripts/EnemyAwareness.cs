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
    public float deaggroBuffer = 2f;
    void Start(){
        enemyAnimator = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();
        playertransform = FindObjectOfType<PlayerMovement>().transform;
    }
    void Update()
    {
        var dist = Vector3.Distance(playertransform.position, transform.position);

        if (!enemy.GetIsStunned())
        {
            bool wasAggro = isAggro;
            //if close aggro
            if (dist <= awarenessRadius)
            {
                isAggro = true;
            }
            //if far deaggro
            else if (dist > awarenessRadius + deaggroBuffer)
            {
                isAggro = false;
            }
            //if it was aggro already dont update
            if (isAggro != wasAggro)
            {
                enemyAnimator.SetBool("isAggro", isAggro);
            }
        }
    }   

}
