using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
public class StaminabarController : MonoBehaviour
{
    [Header("stamina parameters")]
    public float playerStamina = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float embraceCost = 20f;
    [SerializeField] private float dashCost = 20f;
    // [SerializeField] private float slowedSpeed = 4f;
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
        normalSpeed = playerMovement.sprintSpeed;
    }
    private void Update()
    {
        //regen stamina when not running
        if (isSprinting == false)
        {
            if (playerStamina < maxStamina - 0.001f)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                playerStamina = Mathf.Min(playerStamina, maxStamina);
                //update stamina
                UpdateStamina(1);
                if (playerStamina >= maxStamina)
                {
                    playerStamina = maxStamina;
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
        isSprinting = true;
        if (playerStamina > 0)
        {
            
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);
            // if (playerStamina <= 0)
            // {
            //     //slow player
            //     playerMovement.SetRunSpeed(slowedSpeed);
            //     sliderCanvasGroup.alpha = 0;
            //     hasRegenerated = false;
            // }
        }
    }
    public void StaminaEmbrace()
    {
        if (playerStamina >= embraceCost)
        {
            playerStamina -= embraceCost;

        }
        else
        {
            playerStamina = 0;
        }
        isSprinting = false;
        //allow player to embrace
        playerEmbrace.TryEmbrace();
        UpdateStamina(1);
    }

    public void StaminaDash(Vector3 inputDirection)
    {
        if (playerStamina >= dashCost)
        {
            playerStamina -= dashCost;
            playerMovement.StartDash(inputDirection);
            UpdateStamina(1);
        }
        isSprinting = false;
        //allow player to embrace

    }

    public void StaminaRegain(float amount)
    {
        playerStamina += amount;

        // if u go way over set it to maxStamina
        playerStamina = Mathf.Min(playerStamina, maxStamina);

        // if the difference between is less than 0.01 then set it to max
        if (Mathf.Abs(playerStamina - maxStamina) <= 0.01f)
        {
            playerStamina = maxStamina;
        }

        UpdateStamina(1);
    }
}   
