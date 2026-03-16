using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class COntrastEffect : MonoBehaviour
{
    [Header("References")]
    public Volume volume;  // Assign your Post Process Volume here
    
    [Header("Contrast Settings")]
    public float fadeDuration = 1f;
    public float startContrast = -50f; // Example starting value
    public float endContrast = 0f;     // Example ending value

    private ColorAdjustments colorAdjustments;

    void Awake()
    {
        // Get the Color Adjustments override from the volume profile
        if (volume != null && volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.contrast.value = startContrast;
            StartCoroutine(FadeContrast());
        }
        else
        {
            Debug.LogError("No Color Adjustments override found in the assigned Volume.");
        }
    }

    private IEnumerator FadeContrast()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            colorAdjustments.contrast.value = Mathf.Lerp(startContrast, endContrast, t);

            yield return null;
        }

        colorAdjustments.contrast.value = endContrast; // ensure it ends right
    }
    
}
