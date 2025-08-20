using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class VignetteOnAwake : MonoBehaviour
{
[Header("References")]
    public Volume volume;  // Assign your Post Process Volume here
    
    [Header("Vignette Settings")]
    public float fadeDuration = 1f;
    public float startIntensity = 1f; // fully dark
    public float endIntensity = 0f;   // clear screen

    private Vignette vignette;

    void Awake()
    {
        // Get the vignette component from the volume profile
        if (volume != null && volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = startIntensity;
            StartCoroutine(FadeVignette());
        }
        else
        {
            Debug.LogError("No Vignette override found in the assigned Volume.");
        }
    }

    private IEnumerator FadeVignette()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            vignette.intensity.value = Mathf.Lerp(startIntensity, endIntensity, t);

            yield return null;
        }

        vignette.intensity.value = endIntensity; // make sure it finishes clean
    }
}
