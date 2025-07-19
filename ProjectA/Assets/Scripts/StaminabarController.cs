using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
public class StaminabarController : MonoBehaviour
{
    [Header("stamina parameters")]
    public float playerStamina = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float embraceCost = 20f;
    [SerializeField] private float slowedSpeed = 4f;
    [SerializeField] private float normalSpeed = 20f;


    [Header("regen parameters")]
    [SerializeField] private float staminaRegen = 0.5f;
    [SerializeField] private float staminaDrain = 0.5f;

    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool isSprinting = true;

    [Header("UI elements")]
    [SerializeField] private UnityEngine.UI.Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private PlayerMovement playerMovement;
    private PlayerEmbrace playerEmbrace;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerEmbrace = FindObjectOfType<PlayerEmbrace>();
    }
    private void Update()
    {
        //regen stamina when not running
        if (isSprinting == false)
        {
            if (playerStamina <= maxStamina - 0.01f)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                //update stamina
                UpdateStamina(1);
                if (playerStamina >= maxStamina)
                {
                    //set to normal speed
                    playerMovement.SetRunSpeed(normalSpeed);
                    sliderCanvasGroup.alpha = 0;
                    hasRegenerated = true;
                }
            }
        }
    }
    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else if (value == 1)
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
    public void Sprinting()
    {
        Debug.Log("Sprinting called: " + playerStamina);
        if (hasRegenerated)
        {
            isSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);
            if (playerStamina <= 0)
            {
                //slow player
                playerMovement.SetRunSpeed(slowedSpeed);
                sliderCanvasGroup.alpha = 0;
            }
        }
    }
    public void StaminaEmbrace()
    {
        if (playerStamina >= (maxStamina * embraceCost / maxStamina))
        {
            playerStamina -= embraceCost;
            //allow player to embrace
            playerEmbrace.TryEmbrace();
            UpdateStamina(1);
        }
    }

    public void StaminaRegain(float amount)
    {
        if (playerStamina <= maxStamina - 0.01f)
        {
            if (playerStamina + amount >= maxStamina)
            {
                playerStamina = maxStamina;
            }
            else
            {
                playerStamina += amount;
            }
        }
    }
}   
