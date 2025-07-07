using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f, currentHealth;
    [SerializeField] private TakeDamage takeDamage;

    void Start()
    {
        currentHealth = maxHealth;
        takeDamage = FindObjectOfType<TakeDamage>();
    }
    //take damage function
    public void TakeDamage(float damage)
    {
        takeDamage.VignetteEffect();
        currentHealth -= damage;
    }

    //heal function
    public void Heal(float healing)
    {
        //if max health dont do anythin
        if (maxHealth == currentHealth)
        {
            return;
        }//if health plus healing greater than max health set to max health
        else if (currentHealth + healing > maxHealth)
        {
            currentHealth = maxHealth;
        }//else just add it
        else
        {
            currentHealth += healing;
        }

    }
    //set maxhealth function
    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
    }
    public bool Dead()
    {
        if (currentHealth <= 0)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
