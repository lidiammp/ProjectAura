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
    [SerializeField] private float normalSpeed = 20f;

    [Header("regen parameters")]
    [SerializeField] private float staminaRegenStill = 1.5f; 
    [SerializeField] private float staminaRegenMoving = 0.5f;
    [SerializeField] private float staminaDrain = 0.5f;
    [SerializeField] private float regenDelay = 1.0f; 
    [SerializeField] private float fastRegenMultiplier = 2f; // slightly faster than normal
    // [SerializeField] private float emptyStaminaRegen = 0.3f; // slower when at 0

    private float regenTimer = 0f;

    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool isSprinting = true;

    [Header("UI elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    [Header("Cooldown parameters")]
    [SerializeField] private float runCooldownTime = 2f;
    private float cooldownTimer = 0f;
    private bool isCooldown = false;

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
        // Handle cooldown
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                playerMovement.SetRunSpeed(normalSpeed);
            }
        }

        // Stamina regen
        if (!isSprinting && playerStamina < maxStamina)
        {
            if (regenTimer > 0f)
            {
                regenTimer -= Time.deltaTime;
            }
            else
            {
                float regenRate;

                // if (playerStamina <= 0f)
                // {
                //     // Regen slowly if stamina is empty
                //     regenRate = emptyStaminaRegen;
                // }
                // else
                // {
                    // Normal or slightly faster regen
                    regenRate = playerMovement.isMoving ? staminaRegenMoving : staminaRegenStill;
                    regenRate *= fastRegenMultiplier;
                // }

                playerStamina += regenRate * Time.deltaTime;
                playerStamina = Mathf.Min(playerStamina, maxStamina);
                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                    hasRegenerated = true;
            }
        }
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        sliderCanvasGroup.alpha = 1;
    }

    public void Sprinting()
    {
        if (isCooldown) return;

        isSprinting = true;
        if (playerStamina > 0)
        {
            DrainStamina(staminaDrain * Time.deltaTime);
        }
    }

    public void StaminaEmbrace()
    {
        if (playerStamina >= embraceCost)
        {
            DrainStamina(embraceCost);
        }
        else
        {
            playerStamina = 0;
            StartRunCooldown();
        }
        isSprinting = false;
        playerEmbrace.TryEmbrace();
        UpdateStamina(1);
    }

    public void StaminaDash(Vector3 inputDirection)
    {
        float actualDashDistance = playerMovement.GetDashDistance(); 

        if (playerStamina >= dashCost)
        {
            DrainStamina(dashCost);
        }
        else
        {
            playerStamina = 0;
            actualDashDistance = playerMovement.GetMinDashDistance();
            StartRunCooldown();
        }

        playerMovement.StartDash(inputDirection, actualDashDistance);
        UpdateStamina(1);
        isSprinting = false;
    }

    public void StaminaRegain(float amount)
    {
        playerStamina += amount;
        playerStamina = Mathf.Min(playerStamina, maxStamina);
        UpdateStamina(1);
    }

    void StartRunCooldown()
    {
        isCooldown = true;
        cooldownTimer = runCooldownTime;
        playerMovement.SetRunSpeed(1f);
        hasRegenerated = false;
    }

    public void ResetRunCooldown()
    {
        isCooldown = false;
        cooldownTimer = 0;
        hasRegenerated = true;
    }

    public bool GetCoolDown()
    {
        return isCooldown;
    }

    private void DrainStamina(float amount)
    {
        playerStamina -= amount;
        playerStamina = Mathf.Max(playerStamina, 0f);
        regenTimer = regenDelay;
        UpdateStamina(1);
    }
}
