using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public bool isAggro;
    public Material aggroMat;
    private Enemy enemy;
    void Start(){
        enemy = GetComponent<Enemy>();
    }
    void Update(){
        if(isAggro && enemy.isStunned == false){
            GetComponent<MeshRenderer>().material = aggroMat;
        }
    }    
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            isAggro = true;   
        }   
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            isAggro = false;   
        }   
    }
}
