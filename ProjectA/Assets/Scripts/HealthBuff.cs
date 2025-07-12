using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{   
    public float amount;
    
    public override void Apply(GameObject target)
    {
        if(target.tag == "Player"){
            float health = target.GetComponent<Healthbar>().GetCurrentHealth();
            if(target.GetComponent<Healthbar>().GetMaxHealth() != health){
                health += amount;
                target.GetComponent<Healthbar>().SetMaxHealth(health);
            }
        
        }
        
        
    }
}
