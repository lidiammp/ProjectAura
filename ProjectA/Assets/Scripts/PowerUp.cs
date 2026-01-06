using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter(Collider collision){
        //check if player tag
        if (collision.gameObject.tag == "Player")
        {
            powerupEffect.Apply(collision.gameObject);
            AudioManager.instance.PlaySFX("Pickup");
            Destroy(gameObject);
            
        }
        
    }
}
