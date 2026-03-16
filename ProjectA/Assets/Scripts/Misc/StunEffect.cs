using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : MonoBehaviour
{
    [SerializeField]
    private float lifetime;
    private float timer = 0;
    void Start(){
        
    }
    // Start is called before the first frame update
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= lifetime){
            Destroy(gameObject);
        }
    }


}
