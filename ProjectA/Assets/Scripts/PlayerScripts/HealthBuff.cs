using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{   
    public float amount;
    
    public override void Apply(GameObject target)
    {
        if (target.tag == "Player")
        {
            float health = target.GetComponent<Healthbar>().GetCurrentHealth();
            float maxHealth = target.GetComponent<Healthbar>().GetMaxHealth();
            //increase max health
            maxHealth += amount;
            target.GetComponent<Healthbar>().SetMaxHealth(maxHealth);
            //heal
            target.GetComponent<Healthbar>().Heal(maxHealth);
            
        
        }
        
        
    }
}
