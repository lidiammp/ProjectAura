using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnCollision : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportTarget;
    public float teleportDelay = 1f;
    public float doorEffectDelay = 1f;

    [Header("Effects")]
    public ParticleSystem effectPrefab;
    // public AudioClip departureClip;
    // public AudioClip arrivalClip;
    public bool destroyAfter = false;

    PlayerMovement playerMovement;
    CharacterController charController;
    Rigidbody rb;
    [Header("Door")]
    public DoorController door;
    public DoorEffect doorEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            charController = other.GetComponent<CharacterController>();
            rb = other.GetComponent<Rigidbody>();

            playerMovement?.LockMovement();
            StartCoroutine(TeleportSequence(other.gameObject));

        }
    }
    
    private IEnumerator TeleportSequence(GameObject player)
    {
        AudioManager.instance.PlaySFX("Warp");
        // Inside your teleport sequence or OnTriggerEnter
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false; // key is invisible, but script still runs
        }


        // departure effect
        if (effectPrefab != null)
        {
            var effect = Instantiate(effectPrefab, player.transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }

        // if (departureClip != null)
        //     AudioSource.PlayClipAtPoint(departureClip, player.transform.position);

        yield return new WaitForSeconds(teleportDelay);


        // disable movement components before moving
        if (charController != null) charController.enabled = false;
        if (rb != null) rb.isKinematic = true;

        if (teleportTarget != null)
        {
            player.transform.SetPositionAndRotation(teleportTarget.position, teleportTarget.rotation);
        }

        // re-enable movement
        if (charController != null) charController.enabled = true;
        if (rb != null) rb.isKinematic = false;

        playerMovement?.UnlockMovement();
        AudioManager.instance.PlaySFX("WarpZap");
        // arrival effect
        if (effectPrefab != null)
        {
            var effect = Instantiate(effectPrefab, teleportTarget.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax - 1f);
        }

        // if (arrivalClip != null)
        //     AudioSource.PlayClipAtPoint(arrivalClip, teleportTarget.position);
        if (door != null)
        {
            door.SetOpen(true);
        }
        if (destroyAfter)
            Destroy(gameObject);
        yield return new WaitForSeconds(doorEffectDelay);
        gameObject.SetActive(false);
        if (doorEffect != null)
        {
            doorEffect.TriggerEffect();
        }
    }
}

