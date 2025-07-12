using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter(Collider collision){
        //check if player tag
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            powerupEffect.Apply(collision.gameObject);
        }
        
    }
}
