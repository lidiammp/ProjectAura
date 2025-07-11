using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter2D(Collider2D collision){
        //check if player tag
        Destroy(gameObject);
        powerupEffect.Apply(collision.gameObject);
    }
}
