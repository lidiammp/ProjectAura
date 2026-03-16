using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    //set slider to Health
    //player Health
    //health Slider
    private PlayerMovement player;
    private float playerHealth;
    private float playerCurrentHealth;
    private GameObject sliderGameobject;
    private Slider healthbar;
    private void Start()
    {
        sliderGameobject = GameObject.FindWithTag("Player Healthbar");
        healthbar = sliderGameobject.GetComponent<Slider>();
        player = FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {
        playerHealth = player.GetComponent<Healthbar>().GetMaxHealth();
        playerCurrentHealth = player.GetComponent<Healthbar>().GetCurrentHealth();
        SetSlider();
    }
    private void SetSlider()
    {
        healthbar.value = playerCurrentHealth / playerHealth;
    }
}
