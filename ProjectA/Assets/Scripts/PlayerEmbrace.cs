using System.Collections;
using System.Collections.Generic;
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
    private float embraceTimer = 0;
    private PlayerMovement playerMovement;
    [SerializeField] private float invDuration = 0.5f;
    [SerializeField] private GameObject playerCamera;
    private StaminabarController staminabarController;

    private TimeManager timeManager;
    [SerializeField] private float embraceAngle = 45f;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        mouseLook = FindObjectOfType<MouseLook>();
        handAnimator = GetComponent<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
        playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Healthbar>();
        staminabarController = FindObjectOfType<StaminabarController>();
        timeManager = FindObjectOfType<TimeManager>();
    }

    
    // Start is called before the first frame update
    void Update()
    {
        embraceTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E) && embraceTimer > embraceCooldown)
        {

            staminabarController.StaminaEmbrace();
            embraceTimer = 0;
            // isCharging = true;
            // holdTimer = 0f;
            // Debug.Log("warming up hands for hug");
        }
        // HighlightEnemies();

        // if (Input.GetKey(KeyCode.E) && isCharging)
        // {
        //     holdTimer += Time.deltaTime;

        //     if (holdTimer >= chargeTime)
        //     {
        //         Debug.Log("bring it in brother");
        //         Debug.Log($" Embrace charge complete: held for {holdTimer:F2} seconds (needed {chargeTime} seconds).");

        //         TryEmbrace();

        //         // reset
        //         holdTimer = 0f;
        //         isCharging = false;
        //     }
        // }

        // // Cancel charge if player releases early
        // if (Input.GetKeyUp(KeyCode.E) && isCharging)
        // {
        //     Debug.Log("Im not warmed up enough");
        //     holdTimer = 0f;
        //     isCharging = false;
        // }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            var outline = other.GetComponentInChildren<OutlineManager>();
            if (outline != null && enemy.GetIsStunned())
            {
                outline.EnableOutline();
                Debug.Log("incollider");
            }
        
        }
            
    }

    private void OnTriggerStay(Collider other)
    {
        //while in trigger if the enemy is not in angle disable its collider
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            var outline = other.GetComponentInChildren<OutlineManager>();

            if (!enemy.GetIsStunned())
            {
                outline.DisableOutline();
                Debug.Log("Enemy not stunned: outline disabled");
                return;
            }

            if (outline != null && CheckAngle(other.GetComponent<Enemy>()) == false)
            {
                outline.DisableOutline();
                Debug.Log("out of range");
            }
            else
            {
                outline.EnableOutline();
                Debug.Log("in range");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            var outline = other.GetComponentInChildren<OutlineManager>();
            if (outline != null)
            {
                outline.DisableOutline();
                Debug.Log("out of collider");
            }
        }

    }

    public bool CheckAngle(Enemy enemy)
    {
        if (enemy == null) return false;

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;
        Vector3 toTarget = (enemy.transform.position - origin).normalized;

        float dot = Vector3.Dot(forward, toTarget);
        float currentAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return currentAngle <= embraceAngle;
    }
    public void TryEmbrace()
    {
        // Collider[] hits= Physics.OverlapSphere(transform.position, embraceRange, enemyLayer);
        // foreach (var hit in hits)
        // Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        // if (Physics.Raycast(ray, out RaycastHit hit, embraceRange, enemyLayer))
        // {
        //     Enemy enemyComponent = hit.collider.gameObject.GetComponent<Enemy>();
        //     if (enemyComponent != null && enemyComponent.GetIsStunned())
        //     {
        //         EmbraceEnemy(enemyComponent);
        //         return; // laddies leave me alone type shift, one at a time 
        //     }

        // }


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
                    //highlight
                    EmbraceEnemy(hit.GetComponent<Enemy>());
                    huggedSomeone = true;
                    break;
                }
            }

        }
        if (huggedSomeone == false)
        {
            handAnimator.SetTrigger("isHandHugMiss");

        }





        void EmbraceEnemy(Enemy target)
        {
            playerMovement.GetComponent<MouseLook>().RotateToPoint(target.transform);
            // for now we js destroy it or maybe zion can add animation 
            // (pls someone teach me I have ptsd from when i touched animation and deleted everythingggg)
            staminabarController.StaminaRegain(30f);
            //remove from list
            string animationName = target.GetComponent<Enemy>().enemyType;
            handAnimator.SetTrigger("isHandHug" + animationName);

            enemyManager.RemoveEnemy(target);
            target.GetComponent<Animator>().Play("MunchkinPassing");
            //25% chance to heal by healvalue

            playerHealth.Heal(healValue);
            
            
            //target.Die();
            // Debug.Log("HUG!!!");
        }
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

    public void BeginEmbraceCutscene()
    {

        mouseLook.LockMouse();
        playerMovement.LockMovement();
        playerHealth.SetInvincible(true);
        timeManager.SlowMo();
        
    }

    public void EndEmbraceCutscene()
    {
        mouseLook.UnlockMouse();
        playerMovement.UnlockMovement();
        //playerHealth.SetInvincible(false);
        StartCoroutine(LingeringInvincibility(invDuration));
        //reset cooldown
        embraceTimer += embraceCooldown;
    }

    public void BeginMissEmbraceCutscene()
    {
        mouseLook.LockMouse();
        playerMovement.LockMovement();
    }

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
