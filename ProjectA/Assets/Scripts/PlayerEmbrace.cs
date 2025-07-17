using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private PlayerMovement playerMovement;
    [SerializeField] private GameObject playerCamera;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        mouseLook = FindObjectOfType<MouseLook>();
        handAnimator = GetComponent<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
        playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Healthbar>();
    }

    // Start is called before the first frame update
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            //new stuff
            TryEmbrace();

            // isCharging = true;
            // holdTimer = 0f;
            // Debug.Log("warming up hands for hug");
        }


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
    void TryEmbrace()
    {
        // Collider[] hits= Physics.OverlapSphere(transform.position, embraceRange, enemyLayer);
        // foreach (var hit in hits)
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, embraceRange, enemyLayer)){
            Enemy enemyComponent = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemyComponent != null && enemyComponent.GetIsStunned())
            {
                EmbraceEnemy(enemyComponent);
                return; // laddies leave me alone type shift, one at a time 
            }
            
        }else
        {
            handAnimator.Play("HandHugMiss");
        }

        void EmbraceEnemy(Enemy target)
        {
            // for now we js destroy it or maybe zion can add animation 
            // (pls someone teach me I have ptsd from when i touched animation and deleted everythingggg)

            //remove from list
            handAnimator.Play("HandHug");
            enemyManager.RemoveEnemy(target);
            target.GetComponent<Animator>().Play("MunchkinPassing");
            //25% chance to heal by healvalue

            playerHealth.Heal(healValue);


            //target.Die();
            Debug.Log("HUG!!!");
        }
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
    }

    public void EndEmbraceCutscene()
    {
        mouseLook.UnlockMouse();
        playerMovement.UnlockMovement();
    }
}
