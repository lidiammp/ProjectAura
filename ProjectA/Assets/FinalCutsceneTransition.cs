using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCutsceneTransition : MonoBehaviour
{
    BloomTransition bloomTransition;
    void Start(){
        bloomTransition = GetComponent<BloomTransition>();
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && bloomTransition != null){
            other.GetComponent<PlayerMovement>().LockMovement();
            other.GetComponent<MouseLook>().LockMouse();
            bloomTransition.StartTransition();
        }
    }
}
