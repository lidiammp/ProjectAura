using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEmbrace : MonoBehaviour
{
    [SerializeField] private float healValue;
    public float embraceRange = 2f;
    public float chargeTime = 1f;
    public LayerMask enemyLayer;
    private float holdTimer = 0f;
    private bool isCharging = false;
    // public LayerMask enemyLayer;
    private MouseLook mouseLook;
    private EnemyManager enemyManager;
    private Healthbar playerHealth;
    private Animator handAnimator;
    [SerializeField] private float embraceCooldown = 3;
    [SerializeField] private float staminaRegain = 60f;
    private float embraceTimer = 0;
    private PlayerMovement playerMovement;
    [SerializeField] private float invDuration = 0.5f;
    // [SerializeField] private GameObject playerCamera;
    private StaminabarController staminabarController;

    private TimeManager timeManager;
    [SerializeField] private float embraceAngle = 45f;
    public Color embraceRangeColor = new Color(0, 128, 128);
    private EmbraceRetical reticalManager;
    [Header("Snap To Enemy Parameters")]
    // snapRange ← how close you must be to start snapping
    // snapSpeed ← how fast you move toward the enemy
    // targetEnemy ← null or reference to enemy
    // isSnapping ← false
    // characterController ← your movement system
    [SerializeField] private float snapSpeed = 10f;
    [SerializeField] private bool isSnapping = false;
    private bool embraceAnimationEnded = true;
    [SerializeField] private Beam beam;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        mouseLook = FindObjectOfType<MouseLook>();
        handAnimator = GetComponent<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
        playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Healthbar>();
        staminabarController = FindObjectOfType<StaminabarController>();
        timeManager = FindObjectOfType<TimeManager>();
        reticalManager = FindObjectOfType<EmbraceRetical>();
    }

    void EmbraceAnimationEndEvent()
    {
        embraceAnimationEnded = true;
    }
    public void EndEmbraceCutscene()
    {
        mouseLook.UnlockMouse();
        playerMovement.UnlockMovement();
        reticalManager.SetDefaultRetical();
        //playerHealth.SetInvincible(false);
        StartCoroutine(LingeringInvincibility(invDuration));
        //reset cooldown
        embraceTimer += embraceCooldown;

    }
    // Start is called before the first frame update
    void Update()
    {
        embraceTimer += Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)) && embraceTimer > embraceCooldown && isSnapping == false)
        {

            staminabarController.StaminaEmbrace();
            embraceTimer = 0;

        }

        if (embraceAnimationEnded)
        {
            embraceAnimationEnded = false;
            EndEmbraceCutscene();
        }


    }


    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;
        Enemy enemy = other.GetComponent<Enemy>();
        //if no enemy component
        if (enemy == null) return;
        if (enemy.GetIsStunned())
        {
            //eneable outline
            Vector3 toTarget = (enemy.transform.position - playerMovement.transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toTarget);
            float currentAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (currentAngle <= embraceAngle && enemy.GetIsStunned())
            {
                enemy.GetComponentInChildren<OutlineManager>()?.SetOutlineColor(embraceRangeColor);
                reticalManager.SetEmbraceRetical();
            }
            else
            {
                enemy.GetComponentInChildren<OutlineManager>()?.SetOutlineColor(new Color(255, 255, 255));
                reticalManager.SetDefaultRetical();
            }
        }
        else
        {
            enemy.GetComponentInChildren<OutlineManager>()?.DisableOutline();
        }
        //if enemy


    }
    private void OnTriggerExit(Collider other)
    {

        if (((1 << other.gameObject.layer) & enemyLayer) == 0)
        {
            return;
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;
        if (enemy.GetIsStunned())
        {
            enemy.GetComponentInChildren<OutlineManager>()?.SetOutlineColor(new Color(255, 255, 255));
        }
        else
        {
            enemy.GetComponentInChildren<OutlineManager>()?.DisableOutline();
        }
        

    }

    public void TryEmbrace()
    {

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;
        Collider[] hits = Physics.OverlapSphere(origin, embraceRange, enemyLayer);
        bool huggedSomeone = false;
        foreach (var hit in hits)
        {
            Vector3 toTarget = (hit.transform.position - origin).normalized;
            float dot = Vector3.Dot(forward, toTarget);
            float currentAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (currentAngle <= embraceAngle)
            {


                // if enemy and stunned
                if (hit.GetComponent<Enemy>() && hit.GetComponent<Enemy>().GetIsStunned())
                {
                    mouseLook.LockMouse();
                    playerMovement.LockMovement();
                    Enemy enemy = hit.GetComponent<Enemy>();
                    //snap to enemy
                    StartCoroutine(SnapToEnemy(enemy));
                    huggedSomeone = true;
                    break;
                }
            }

        }
        if (huggedSomeone == false)
        {
            handAnimator.SetTrigger("isHandHugMiss");
        }
    
    }
    IEnumerator SnapToEnemy(Enemy enemy)
    {
        isSnapping = true;
        
        // playerMovement.LockMovement();
        while ((enemy.transform.position - transform.position).magnitude > enemy.snapThreshold)
        {
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            playerMovement.Move(snapSpeed, direction);
            playerMovement.GetComponent<MouseLook>().RotateToPoint(enemy.transform);
            yield return null;
        }

        isSnapping = false;
        timeManager.Stop(0.75f);
        EmbraceEnemy(enemy);
    }
    void EmbraceEnemy(Enemy target)
    {   
        playerHealth.SetInvincible(true);
        enemyManager.RemoveEnemy(target);
        
        // for now we js destroy it or maybe zion can add animation 
        // (pls someone teach me I have ptsd from when i touched animation and deleted everythingggg)
        staminabarController.StaminaRegain(staminaRegain);
        staminabarController.ResetRunCooldown();
        //remove from list
        string animationName = target.GetComponent<Enemy>().enemyType;
        handAnimator.SetTrigger("isHandHug" + animationName);

        
        target.GetComponent<Animator>().Play("MunchkinPassing");
        //25% chance to heal by healvalue

        playerHealth.Heal(healValue);
        reticalManager.SetDefaultRetical();
        
        //target.Die();
        // Debug.Log("HUG!!!");
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, embraceRange);

        Vector3 left = Quaternion.Euler(0, -embraceAngle, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, embraceAngle, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + left * embraceRange);
        Gizmos.DrawLine(transform.position, transform.position + right * embraceRange);
    }
    public float GetHeldTime()
    {
        return holdTimer;
    }

    public bool IsCharging()
    {
        return isCharging;
    }




    // public void BeginMissEmbraceCutscene()
    // {
    //     mouseLook.LockMouse();
    //     playerMovement.LockMovement();
    // }

    public void EndEmbraceMissCutscene()
    {
        mouseLook.UnlockMouse();
        playerMovement.UnlockMovement();
    }

    IEnumerator LingeringInvincibility(float duration)
    {
        // Debug.Log("Invincibility ON");
        playerHealth.SetInvincible(true);
        yield return new WaitForSeconds(duration);
        // Debug.Log("Invincibility OFF");
        playerHealth.SetInvincible(false);
    }
}
