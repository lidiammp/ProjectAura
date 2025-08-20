using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEffect : MonoBehaviour
{
    [Header("Settings")]
    public GameObject targetObject;      // Object to toggle off
    public ParticleSystem effectPrefab;  // Particle effect prefab
    public bool destroyAfterEffect = true; // Destroy effect after it finishes

    public void TriggerEffect()
    {
        if (targetObject != null)
            targetObject.SetActive(false); // Turn off the object

        if (effectPrefab != null)
        {
            // Spawn the effect at the object's position
            ParticleSystem effect = Instantiate(effectPrefab, targetObject.transform.position, Quaternion.identity);
            effect.Play();

            // Destroy effect after it finishes
            if (destroyAfterEffect)
            {
                Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
            }
        }
    }
}
