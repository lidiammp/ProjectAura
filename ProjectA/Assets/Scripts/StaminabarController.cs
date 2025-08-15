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

    [SerializeField] private float staminaRegenStill = 1.5f; // Faster regen when still
    [SerializeField] private float staminaRegenMoving = 0.5f; // Normal regen when moving

    // [SerializeField] private float staminaRegen = 0.5f;
    [SerializeField] private float staminaDrain = 0.5f;

    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool isSprinting = true;

    [Header("UI elements")]
    [SerializeField] private UnityEngine.UI.Image staminaProgressUI = null;
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
        // Handle cooldown timer
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                playerMovement.SetRunSpeed(normalSpeed);
            }
        }

        // Regen stamina when not sprinting
        if (!isSprinting)
        {
            if (playerStamina < maxStamina - 0.001f)
            {
                bool isMoving = playerMovement.isMoving;


                float regenRate = isMoving ? staminaRegenMoving : staminaRegenStill;

                playerStamina += regenRate * Time.deltaTime;
                playerStamina = Mathf.Min(playerStamina, maxStamina);

                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                {
                    playerStamina = maxStamina;
                    hasRegenerated = true;
                }
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
        if (isCooldown) return; // no sprint if on cooldon

        isSprinting = true;
        if (playerStamina > 0)
        {
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                playerStamina = 0;
                StartRunCooldown();
            }
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
            StartRunCooldown();
        }
        isSprinting = false;
        playerEmbrace.TryEmbrace();
        UpdateStamina(1);
    }

    public void StaminaDash(Vector3 inputDirection)
    {
        if (isCooldown) return; // no dash on cooldown

        if (playerStamina >= dashCost)
        {
            playerStamina -= dashCost;
            playerMovement.StartDash(inputDirection);
            UpdateStamina(1);
        }
        
        isSprinting = false;
    }

    public void StaminaRegain(float amount)
    {
        playerStamina += amount;
        playerStamina = Mathf.Min(playerStamina, maxStamina);

        if (Mathf.Abs(playerStamina - maxStamina) <= 0.01f)
        {
            playerStamina = maxStamina;
        }
        UpdateStamina(1);
    }

    void StartRunCooldown()
    {
        isCooldown = true;
        cooldownTimer = runCooldownTime;
        playerMovement.SetRunSpeed(0f); // Or slow down instead of stopping completely
        hasRegenerated = false;
    }
    public bool GetCoolDown(){
        return isCooldown;
    }
}   
